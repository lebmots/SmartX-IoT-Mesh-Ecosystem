using System.Windows;

namespace SmartX.Desktop
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        // Opens the Sensor Data Ingestion and Telemetry section.
        //
        // This functionality will later navigate to the Smart-X
        // telemetry dashboard where registered ESP32 devices,
        // telemetry readings and device-health information are displayed.
        
        private void btnTelemetry_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
            "Sensor Data Ingestion and Telemetry module selected.",
            "Smart-X IoT Gateway",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        }
    }
}
