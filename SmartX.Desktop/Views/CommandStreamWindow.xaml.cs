using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SmartX.Desktop.Services;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartX.Desktop.Views
{
    /// <summary>
    /// Interaction logic for CommandStreamWindow.xaml
    /// </summary>
    public partial class CommandStreamWindow : Window
    {

        public CommandStreamWindow()
        {
            InitializeComponent();

            Loaded += async (_, _) => await LoadFilteredDevicesAsync();
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



    }
}
