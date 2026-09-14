using System.Windows;
using SmartX.Desktop.Models;
using SmartX.Desktop.Services;

namespace SmartX.Desktop.Views
{
    public partial class RegisterDeviceWindow : Window
    {
        private readonly SmartXApiService _apiService;

        public bool DeviceRegistered { get; private set; }

        public RegisterDeviceWindow()
        {
            InitializeComponent();

            _apiService = new SmartXApiService();
        }

        private async void btnRegister_Click(
        object sender,
        RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeviceId.Text) ||
            string.IsNullOrWhiteSpace(txtMacAddress.Text) ||
            string.IsNullOrWhiteSpace(txtDeploymentLocation.Text))
            {
                MessageBox.Show(
                "Please complete all device details.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

                return;
            }

            DeviceRegistrationDto request = new()
            {
                DeviceId = txtDeviceId.Text.Trim(),
                MacAddress = txtMacAddress.Text.Trim(),
                DeploymentLocation =
            txtDeploymentLocation.Text.Trim(),

                Category = cmbCategory.SelectedIndex
            };

            try
            {
                var result =
                await _apiService.RegisterDeviceAsync(request);

                if (!result.Success)
                {
                    MessageBox.Show(
                    $"Unable to register device.\n\n{result.Message}",
                    "Registration Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                    return;
                }

                DeviceRegistered = true;

                MessageBox.Show(
                "Device registered successfully.",
                "Smart-X",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                Close();
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

        private void btnCancel_Click(
        object sender,
        RoutedEventArgs e)
        {
            Close();
        }
    }
}





