

using System.Windows;
using SmartX.Desktop.Services;

namespace SmartX.Desktop.Views
{
    public partial class TelemetryWindow : Window
    {
        private readonly SmartXApiService _apiService;

        public TelemetryWindow()
        {
            InitializeComponent();

            _apiService = new SmartXApiService();

            Loaded += TelemetryWindow_Loaded;
        }

        private async void TelemetryWindow_Loaded(
        object sender,
        RoutedEventArgs e)
        {
            await LoadDevicesAsync();
        }

        private async void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
        {
            await LoadDevicesAsync();
        }

        private async Task LoadDevicesAsync()
        {
            try
            {
                dgDevices.ItemsSource =
                await _apiService.GetDevicesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Unable to connect to the Smart-X API.\n\n{ex.Message}",
                "Connection Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }
    }
}


