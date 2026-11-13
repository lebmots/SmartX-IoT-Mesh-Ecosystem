using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;

namespace SmartX.Desktop.Views
{


    public class BfsResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";

        public List<string> VisitedNodes { get; set; } = new();

        public Dictionary<string, int> HopCounts { get; set; } = new();

        public List<string> UnreachableNodes { get; set; } = new();
    }

    public class MstEdgeResponse
    {
        public string SourceId { get; set; } = "";
        public string DestinationId { get; set; } = "";
        public double Weight { get; set; }
    }

    public class MstResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        [System.Text.Json.Serialization.JsonPropertyName("selectedConnections")]
        public List<MstEdgeResponse> Edges { get; set; } = new();
        public double TotalWeight { get; set; }
        public List<string> VisitedNodes { get; set; } = new();
        public List<string> UnreachableNodes { get; set; } = new();
    }

    public class DijkstraResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public List<string> Path { get; set; } = new();
        public double TotalCost { get; set; }
        public int HopCount { get; set; }
    }


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



        private async void btnBfs_Click(
        object sender,
        RoutedEventArgs e)
        {
            btnBfs.IsEnabled = false;
            txtAlgorithmResult.Text = "Running BFS traversal...";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<BfsResponse>(
                "api/mesh/bfs/GATEWAY-001");

                if (result == null)
                {
                    txtAlgorithmResult.Text = "No BFS result returned.";
                    return;
                }

                if (!result.Success)
                {
                    txtAlgorithmResult.Text = result.Message;
                    return;
                }

                string visited = string.Join(
                " → ",
                result.VisitedNodes);

                string unreachable = result.UnreachableNodes.Count == 0
                ? "None"
                : string.Join(", ", result.UnreachableNodes);

                txtAlgorithmResult.Text =
                $"BFS Traversal Completed\n\n" +
                $"Visit order: {visited}\n\n" +
                $"Visited nodes: {result.VisitedNodes.Count}\n" +
                $"Unreachable nodes: {unreachable}";
            }
            catch (Exception ex)
            {
                txtAlgorithmResult.Text =
                $"BFS traversal failed: {ex.Message}";
            }
            finally
            {
                btnBfs.IsEnabled = true;
            }
        }


        private async void btnPrim_Click(
        object sender,
        RoutedEventArgs e)
        {
            btnPrim.IsEnabled = false;
            txtAlgorithmResult.Text = "Calculating Prim's MST...";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<MstResponse>(
                "api/mesh/mst/GATEWAY-001");

                if (result == null)
                {
                    txtAlgorithmResult.Text = "No MST result returned.";
                    return;
                }

                if (!result.Success)
                {
                    txtAlgorithmResult.Text = result.Message;
                    return;
                }

                string connections = string.Join(
                "\n",
                result.Edges.Select(edge =>
                $"{edge.SourceId} → {edge.DestinationId} " +
                $"(Weight: {edge.Weight})"));

                string unreachable = result.UnreachableNodes.Count == 0
                ? "None"
                : string.Join(", ", result.UnreachableNodes);

                txtAlgorithmResult.Text =
                $"Prim's Minimum Spanning Tree Completed\n\n" +
                $"Selected connections:\n{connections}\n\n" +
                $"Total weight: {result.TotalWeight}\n" +
                $"Visited nodes: {result.VisitedNodes.Count}\n" +
                $"Unreachable nodes: {unreachable}";
            }
            catch (Exception ex)
            {
                txtAlgorithmResult.Text =
                $"Prim's MST failed: {ex.Message}";
            }
            finally
            {
                btnPrim.IsEnabled = true;
            }
        }



        private async void btnDijkstra_Click(
        object sender,
        RoutedEventArgs e)
        {
            btnDijkstra.IsEnabled = false;
            txtAlgorithmResult.Text = "Calculating shortest path...";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<DijkstraResponse>(
                "api/mesh/shortest-path/GATEWAY-001/ESP32-0004");

                if (result == null)
                {
                    txtAlgorithmResult.Text = "No shortest-path result returned.";
                    return;
                }

                if (!result.Success)
                {
                    txtAlgorithmResult.Text = result.Message;
                    return;
                }

                string path = string.Join(" → ", result.Path);

                txtAlgorithmResult.Text =
                $"Dijkstra's Shortest Path Completed\n\n" +
                $"Route: {path}\n\n" +
                $"Total cost: {result.TotalCost}\n" +
                $"Hop count: {result.HopCount}";
            }
            catch (Exception ex)
            {
                txtAlgorithmResult.Text =
                $"Dijkstra calculation failed: {ex.Message}";
            }
            finally
            {
                btnDijkstra.IsEnabled = true;
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
}

