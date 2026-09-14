

using Microsoft.Win32;
using SmartX.Desktop.Models;
using SmartX.Desktop.Services;
using System.Windows;
using System.Windows.Threading;

namespace SmartX.Desktop.Views
{
    public partial class TelemetryWindow : Window
    {
        private readonly SmartXApiService _apiService;
        private readonly DispatcherTimer _healthRefreshTimer;
        private bool _isRefreshing;


        public TelemetryWindow()
        {
            InitializeComponent();

            _apiService = new SmartXApiService();


            _healthRefreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            _healthRefreshTimer.Tick += HealthRefreshTimer_Tick;

            Loaded += TelemetryWindow_Loaded;
            Closed += TelemetryWindow_Closed;
        }
        

        private async void TelemetryWindow_Loaded(
        object sender,
        RoutedEventArgs e)
        {
            await LoadDevicesAsync();
            _healthRefreshTimer.Start();
        }


        private async void HealthRefreshTimer_Tick(
        object? sender,
        EventArgs e)
        {
            await LoadDevicesAsync();
        }


        private void TelemetryWindow_Closed(
        object? sender,
        EventArgs e)
        {
            _healthRefreshTimer.Stop();
        }



        private async void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
        {
            await LoadDevicesAsync();
            if (!_healthRefreshTimer.IsEnabled)
            {
                _healthRefreshTimer.Start();
            }
        }

        private async Task LoadDevicesAsync()
        {
        if (_isRefreshing)
        {
                return;
        }
            _isRefreshing = true;

            try
            {
                var devices =
                await _apiService.GetDevicesAsync();

                var anomalies = await _apiService.GetAnomaliesAsync();

                dgDevices.ItemsSource = devices;

                dgAnomalies.ItemsSource = anomalies;

                txtAnomalyCount.Text =
                anomalies.Count == 1
                ? "1 anomaly"
                : $"{anomalies.Count} anomalies";

                txtTotalDevices.Text =
                devices.Count.ToString();

                txtHealthy.Text =
                devices.Count(d => d.HealthStatus == 0).ToString();

                txtWarning.Text =
                devices.Count(d => d.HealthStatus == 1).ToString();

                txtCritical.Text =
                devices.Count(d => d.HealthStatus == 2).ToString();

                txtDisconnected.Text =
                devices.Count(d => d.HealthStatus == 3).ToString();

                txtLastRefresh.Text =
                $"Updated {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to connect to the Smart-X API.\n\n{ex.Message}",
                "Connection Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            finally
            {
                _isRefreshing = false;
            }


        }

        private async void btnRegisterDevice_Click(
        object sender,
        RoutedEventArgs e)
        {
            RegisterDeviceWindow registerWindow = new()
            {
                Owner = this
            };

            registerWindow.ShowDialog();

            if (registerWindow.DeviceRegistered)
            {
                await LoadDevicesAsync();
            }
        }

        private async void btnUploadAttachment_Click(
        object sender,
        RoutedEventArgs e)
        {
            if (dgDevices.SelectedItem is not SensorDeviceDto selectedDevice)
            {
                MessageBox.Show(
                "Please select a device from the table first.",
                "Select Device",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                return;
            }

            OpenFileDialog dialog = new()
            {
                Title = "Select Smart-X Device Attachment",
                Filter =
            "Supported Files|*.txt;*.log;*.json;*.xml;*.csv;*.jpg;*.jpeg;*.png|" +
            "All Files|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result != true)
            {
                return;
            }

            string extension =
            System.IO.Path.GetExtension(dialog.FileName)
            .ToLower();

            string category;

            if (extension == ".jpg" ||
            extension == ".jpeg" ||
            extension == ".png")
            {
                category = "DeploymentPhoto";
            }
            else if (extension == ".log" ||
            extension == ".txt")
            {
                category = "HardwareLog";
            }
            else
            {
                category = "Configuration";
            }

            try
            {
                var uploadResult =
                await _apiService.UploadAttachmentAsync(
                selectedDevice.DeviceId,
                dialog.FileName,
                category);

                if (uploadResult.Success)
                {
                    MessageBox.Show(
                    $"Attachment uploaded successfully.\n\nCategory: {category}",
                    "Smart-X",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                    $"Upload failed.\n\n{uploadResult.Message}",
                    "Upload Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to upload attachment.\n\n{ex.Message}",
                "Connection Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }


    }
}


