

using System.Windows;
using System.Windows.Threading;
using SmartX.Desktop.Services;

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

                dgDevices.ItemsSource = devices;

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

    }
}


