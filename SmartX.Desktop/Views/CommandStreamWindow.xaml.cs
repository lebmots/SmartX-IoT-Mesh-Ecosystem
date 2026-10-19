using SmartX.Desktop.Models;
using SmartX.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SmartX.Desktop.Views
{
   
    public partial class CommandStreamWindow : Window
    {
        public CommandStreamWindow()
        {
            InitializeComponent();

            _telemetryTimer.Interval = TimeSpan.FromSeconds(3);

            _telemetryTimer.Tick += async (_, _) =>
            {
                await RefreshTelemetryDashboardAsync();
                await RefreshCommandsAndRecommendationsAsync();
            };

            Loaded += async (_, _) =>
            {
                await LoadFilteredDevicesAsync();
                await RefreshTelemetryDashboardAsync();
                await RefreshCommandsAndRecommendationsAsync();

                _telemetryTimer.Start();
            };

            Closed += (_, _) => _telemetryTimer.Stop();
        }

        private readonly SmartXApiService _apiService = new();

        private async void btnSearchDevices_Click(object sender, RoutedEventArgs e)
        {
            await LoadFilteredDevicesAsync();
        }

        private async Task LoadFilteredDevicesAsync()
        {
            try
            {
                string search = txtDeviceSearch.Text.Trim();

                string category =
                (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString()
                ?? "All";

                string alertState =
                (cmbAlertState.SelectedItem as ComboBoxItem)?.Content?.ToString()
                ?? "All";

                var devices = await _apiService.GetCommandCentreDevicesAsync(
                search, category, alertState);

                dgFilteredDevices.ItemsSource = devices;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to load devices: {ex.Message}",
                "Smart-X",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }


        private readonly DispatcherTimer _telemetryTimer = new();
        private bool _isRefreshingTelemetry;

        private async Task RefreshTelemetryDashboardAsync()
        {
            if (_isRefreshingTelemetry)
                return;

            _isRefreshingTelemetry = true;

            try
            {
                var queueStatus = await _apiService.GetQueueStatusAsync();
                var telemetry = await _apiService.GetProcessedTelemetryAsync();
                var alerts = await _apiService.GetActiveAlertsAsync();
                var devices = await _apiService.GetCommandCentreDevicesAsync();

                txtQueuedTelemetry.Text = queueStatus.TotalQueued.ToString();
                txtCriticalAlerts.Text = alerts.TotalActiveAlerts.ToString();

                txtActiveDevices.Text = devices.Count(d =>
                !d.Health.Equals(
                "Disconnected",
                StringComparison.OrdinalIgnoreCase)).ToString();

                dgTelemetry.ItemsSource = telemetry
                .OrderByDescending(t => t.ReceivedAt)
                .Take(50)
                .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                $"Telemetry dashboard refresh failed: {ex.Message}");
            }
            finally
            {
                _isRefreshingTelemetry = false;
            }
        }




        private bool _isSendingCommand;

        private async Task SendDeviceCommandAsync(
        string commandType,
        bool turnOn)
        {
            if (_isSendingCommand)
                return;

            // The operator must select a device first.
            if (dgFilteredDevices.SelectedItem
            is not CommandCentreDeviceDto selectedDevice)
            {
                MessageBox.Show(
                "Please select a device from the Device Search table first.",
                "Select Device",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                return;
            }

            string deviceId = selectedDevice.DeviceId;

            string action = turnOn ? "ON" : "OFF";

            var confirmation = MessageBox.Show(
            $"Send {commandType} {action} to {deviceId}?",
            "Confirm Manual Override",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            _isSendingCommand = true;

            btnPumpOn.IsEnabled = false;
            btnPumpOff.IsEnabled = false;
            btnRelayOn.IsEnabled = false;
            btnRelayOff.IsEnabled = false;

            try
            {
                var request = new DeviceCommandRequestDto
                {
                    DeviceId = deviceId,
                    CommandType = commandType,
                    TurnOn = turnOn
                };

                var result = await _apiService.ExecuteDeviceCommandAsync(
                request);

                MessageBox.Show(
                $"{result.CommandType} {action} command recorded " +
                $"for {result.DeviceId}.",
                "Command Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to execute command: {ex.Message}",
                "Command Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            finally
            {
                _isSendingCommand = false;

                btnPumpOn.IsEnabled = true;
                btnPumpOff.IsEnabled = true;
                btnRelayOn.IsEnabled = true;
                btnRelayOff.IsEnabled = true;
            }
        }

        private async void btnPumpOn_Click(
        object sender, RoutedEventArgs e)
        {
            await SendDeviceCommandAsync("Pump", true);
        }

        private async void btnPumpOff_Click(
        object sender, RoutedEventArgs e)
        {
            await SendDeviceCommandAsync("Pump", false);
        }

        private async void btnRelayOn_Click(
        object sender, RoutedEventArgs e)
        {
            await SendDeviceCommandAsync("Relay", true);
        }

        private async void btnRelayOff_Click(
        object sender, RoutedEventArgs e)
        {
            await SendDeviceCommandAsync("Relay", false);
        }



        private async void btnUndo_Click(
        object sender, RoutedEventArgs e)
        {
            if (_isSendingCommand)
                return;

            var confirmation = MessageBox.Show(
            "Undo the most recent manual device command?",
            "Confirm Undo",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
                return;

            _isSendingCommand = true;

            btnUndo.IsEnabled = false;
            btnPumpOn.IsEnabled = false;
            btnPumpOff.IsEnabled = false;
            btnRelayOn.IsEnabled = false;
            btnRelayOff.IsEnabled = false;

            try
            {
                var undone = await _apiService.UndoLastCommandAsync();

                string restoredState =
                undone.PreviousState ? "ON" : "OFF";

                MessageBox.Show(
                $"Successfully undid the last command.\n\n" +
                $"Device: {undone.DeviceId}\n" +
                $"Component: {undone.CommandType}\n" +
                $"Restored state: {restoredState}",
                "Undo Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                ex.StatusCode == System.Net.HttpStatusCode.NotFound
                ? "There are no commands available to undo."
                : $"Unable to undo command: {ex.Message}",
                "Undo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to undo command: {ex.Message}",
                "Undo Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            finally
            {
                _isSendingCommand = false;

                btnUndo.IsEnabled = true;
                btnPumpOn.IsEnabled = true;
                btnPumpOff.IsEnabled = true;
                btnRelayOn.IsEnabled = true;
                btnRelayOff.IsEnabled = true;
            }
        }

        private async Task RefreshCommandsAndRecommendationsAsync()
        {
            try
            {
                var commandHistory =
                await _apiService.GetCommandHistoryAsync();

                var recommendations =
                await _apiService.GetRecommendationsAsync();

                // Update the Commands Issued dashboard counter.
                txtCommandsIssued.Text =
                commandHistory.Count.ToString();

                // Show the latest manual commands.
                dgCommandHistory.ItemsSource = commandHistory
                .OrderByDescending(c => c.ExecutedAt)
                .Take(30)
                .ToList();

                // Display predictive recommendations.
                lstSuggestedActions.ItemsSource = recommendations.Suggestions
                .Select(s =>
                $"[{s.Priority}] {s.DeviceId} — {s.Title}\n" +
                $"{s.Description}\n" +
                $"Suggested action: {s.RecommendedCommand}")
                .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                $"Unable to refresh commands and recommendations: {ex.Message}");
            }
        }


    }
}
