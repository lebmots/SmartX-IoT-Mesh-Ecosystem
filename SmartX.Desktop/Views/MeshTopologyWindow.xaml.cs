using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace SmartX.Desktop.Views
{
    public partial class MeshTopologyWindow : Window
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5016/")
        };

        public MeshTopologyWindow()
        {
            InitializeComponent();

            Loaded += MeshTopologyWindow_Loaded;
            Closed += (_, _) => _httpClient.Dispose();
        }

        private async void MeshTopologyWindow_Loaded(
        object sender,
        RoutedEventArgs e)
        {
            await LoadMeshTopologyAsync();
        }

        private async System.Threading.Tasks.Task LoadMeshTopologyAsync()
        {
            try
            {
                txtNetworkStatus.Text = "Connecting...";

                var topology = await _httpClient.GetFromJsonAsync<MeshTopologyResponse>(
                "api/mesh/topology");

                if (topology == null)
                {
                    txtNetworkStatus.Text = "No data";
                    return;
                }

                txtTotalNodes.Text = topology.TotalNodes.ToString();
                txtConnections.Text = topology.TotalConnections.ToString();
                txtNetworkStatus.Text = "Connected";

                dgMeshNodes.ItemsSource = topology.Nodes;
                dgMeshConnections.ItemsSource = topology.Connections;
            }
            catch (Exception ex)
            {
                txtNetworkStatus.Text = "Disconnected";

                MessageBox.Show(
                $"Unable to load mesh topology.\n\n{ex.Message}",
                "Smart-X API Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            }
        }
    }

    public class MeshTopologyResponse
    {
        public int TotalNodes { get; set; }

        public int TotalConnections { get; set; }

        public List<MeshNodeDto> Nodes { get; set; } = new();

        public List<MeshConnectionDto> Connections { get; set; } = new();
    }

    public class MeshNodeDto
    {
        public string NodeId { get; set; } = "";

        public string MacAddress { get; set; } = "";

        public bool IsOnline { get; set; }

        public bool IsGateway { get; set; }
    }

    public class MeshConnectionDto
    {
        public string SourceId { get; set; } = "";

        public string DestinationId { get; set; } = "";

        public double Weight { get; set; }
    }
}

