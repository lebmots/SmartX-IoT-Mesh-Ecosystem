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

        private async void btnDisconnectNode_Click(
object sender,
RoutedEventArgs e)
        {
            await UpdateNodeStatusAsync(false);
        }

        private async void btnReconnectNode_Click(
        object sender,
        RoutedEventArgs e)
        {
            await UpdateNodeStatusAsync(true);
        }

        private async Task UpdateNodeStatusAsync(bool isOnline)
        {
            if (cmbFaultDevice.SelectedItem is not
            System.Windows.Controls.ComboBoxItem selectedItem)
            {
                txtFaultStatus.Text = "Please select an ESP32 device.";
                return;
            }

            string nodeId = selectedItem.Content?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(nodeId))
                return;

            btnDisconnectNode.IsEnabled = false;
            btnReconnectNode.IsEnabled = false;

            try
            {
                txtFaultStatus.Text = isOnline
                ? $"Reconnecting {nodeId}..."
                : $"Disconnecting {nodeId}...";

                string url =
                $"api/mesh/nodes/{Uri.EscapeDataString(nodeId)}/status?isOnline={isOnline.ToString().ToLowerInvariant()}";

                using var response =
                await _httpClient.PutAsync(url, null);

                response.EnsureSuccessStatusCode();

                txtFaultStatus.Text = isOnline
                ? $"{nodeId} successfully reconnected."
                : $"{nodeId} successfully disconnected.";

                await LoadMeshTopologyAsync();
                await RecalculateRouteAsync();
                await UpdateNetworkRecoveryStatusAsync();
            }
            catch (Exception ex)
            {
                txtFaultStatus.Text =
                $"Unable to update device status: {ex.Message}";
            }
            finally
            {
                btnDisconnectNode.IsEnabled = true;
                btnReconnectNode.IsEnabled = true;
            }
        }


        private async Task RecalculateRouteAsync()
        {
            try
            {
                txtAlgorithmResult.Text =
                "Recalculating shortest route after network change...";

                var result = await _httpClient.GetFromJsonAsync<DijkstraResponse>(
                "api/mesh/shortest-path/GATEWAY-001/ESP32-0004");

                if (result == null)
                {
                    txtAlgorithmResult.Text =
                    "No routing result was returned.";
                    return;
                }

                if (!result.Success)
                {
                    txtAlgorithmResult.Text =
                    $"Route unavailable: {result.Message}";
                    return;
                }

                string route = string.Join(" → ", result.Path);

                txtAlgorithmResult.Text =
                $"Dynamic Route Recalculation Completed\n\n" +
                $"Route: {route}\n\n" +
                $"Total cost: {result.TotalCost}\n" +
                $"Hop count: {result.HopCount}";
            }
            catch (Exception ex)
            {
                txtAlgorithmResult.Text =
                $"Unable to recalculate route: {ex.Message}";
            }
        }

        private async Task UpdateNetworkRecoveryStatusAsync()
        {
            try
            {
                // Get the current mesh topology.
                using var topologyResponse = await _httpClient.GetAsync(
                "api/mesh/topology");

                topologyResponse.EnsureSuccessStatusCode();

                using var topologyJson = await System.Text.Json.JsonDocument
                .ParseAsync(await topologyResponse.Content.ReadAsStreamAsync());

                var nodes = topologyJson.RootElement.GetProperty("nodes");

                int offlineCount = 0;

                foreach (var node in nodes.EnumerateArray())
                {
                    if (!node.GetProperty("isOnline").GetBoolean())
                    {
                        offlineCount++;
                    }
                }

                // Check whether the gateway can reach ESP32-0004.
                var route = await _httpClient.GetFromJsonAsync<DijkstraResponse>(
                "api/mesh/shortest-path/GATEWAY-001/ESP32-0004");

                if (route == null)
                {
                    throw new Exception("No routing information returned.");
                }

                if (!route.Success)
                {
                    brdNetworkRecovery.Background =
                    new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(254, 226, 226));

                    txtRecoveryHeading.Foreground =
                    System.Windows.Media.Brushes.DarkRed;

                    txtRecoveryMessage.Foreground =
                    System.Windows.Media.Brushes.DarkRed;

                    txtRecoveryHeading.Text =
                    "NETWORK FAILURE - Destination Unreachable";

                    txtRecoveryMessage.Text =
                    "The gateway cannot reach ESP32-0004. " +
                    "No valid route is currently available. " +
                    "Reconnect the affected devices to restore connectivity.";
                }
                else if (offlineCount > 0)
                {
                    brdNetworkRecovery.Background =
                    new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(254, 249, 195));

                    txtRecoveryHeading.Foreground =
                    System.Windows.Media.Brushes.DarkGoldenrod;

                    txtRecoveryMessage.Foreground =
                    System.Windows.Media.Brushes.DarkGoldenrod;

                    txtRecoveryHeading.Text =
                    "NETWORK DEGRADED - Rerouting Active";

                    txtRecoveryMessage.Text =
                    $"{offlineCount} device(s) offline. " +
                    $"An available route to ESP32-0004 has been found. " +
                    $"Current route cost: {route.TotalCost}.";
                }
                else
                {
                    brdNetworkRecovery.Background =
                    new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(220, 252, 231));

                    txtRecoveryHeading.Foreground =
                    System.Windows.Media.Brushes.DarkGreen;

                    txtRecoveryMessage.Foreground =
                    System.Windows.Media.Brushes.DarkGreen;

                    txtRecoveryHeading.Text =
                    "NETWORK HEALTHY - All Devices Connected";

                    txtRecoveryMessage.Text =
                    $"All mesh devices are online. " +
                    $"The gateway can reach ESP32-0004 successfully. " +
                    $"Current route cost: {route.TotalCost}.";
                }
            }
            catch (Exception ex)
            {
                brdNetworkRecovery.Background =
                new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(226, 232, 240));

                txtRecoveryHeading.Foreground =
                System.Windows.Media.Brushes.DarkSlateGray;

                txtRecoveryMessage.Foreground =
                System.Windows.Media.Brushes.DarkSlateGray;

                txtRecoveryHeading.Text = "NETWORK STATUS UNKNOWN";

                txtRecoveryMessage.Text =
                $"Unable to verify network health: {ex.Message}";
            }
        }

        public class HistoricalTelemetryReading
        {
            public string DeviceId { get; set; } = "";
            public string Metric { get; set; } = "";
            public double Value { get; set; }
            public string Unit { get; set; } = "";
            public DateTime ReceivedAt { get; set; }
        }

        public class HistoricalTelemetryResponse
        {
            public string DeviceId { get; set; } = "";
            public int TotalReadings { get; set; }

            public List<HistoricalTelemetryReading> Readings { get; set; }
            = new();
        }


        private async void btnSearchHistory_Click(
        object sender,
        RoutedEventArgs e)
        {
            if (cmbHistoryDevice.SelectedItem is not
            System.Windows.Controls.ComboBoxItem selectedItem)
            {
                txtHistoryStatus.Text = "Please select an ESP32 device.";
                return;
            }

            string deviceId = selectedItem.Content?.ToString() ?? "";

            if (!dpHistoryStart.SelectedDate.HasValue ||
            !dpHistoryEnd.SelectedDate.HasValue)
            {
                txtHistoryStatus.Text =
                "Please select both a start date and an end date.";
                return;
            }

            DateTime startDate = dpHistoryStart.SelectedDate.Value.Date;
            DateTime endDate = dpHistoryEnd.SelectedDate.Value.Date
            .AddDays(1).AddTicks(-1);

            if (startDate > endDate)
            {
                txtHistoryStatus.Text =
                "Start date cannot be after end date.";
                return;
            }

            btnSearchHistory.IsEnabled = false;
            txtHistoryStatus.Text = "Searching AVL Tree...";
            dgHistoryReadings.ItemsSource = null;

            try
            {
                string url =
                $"api/avl/history?deviceId={Uri.EscapeDataString(deviceId)}" +
                $"&start={Uri.EscapeDataString(startDate.ToString("yyyy-MM-ddTHH:mm:ss"))}" +
                $"&end={Uri.EscapeDataString(endDate.ToString("yyyy-MM-ddTHH:mm:ss"))}";

                var result = await _httpClient
                .GetFromJsonAsync<HistoricalTelemetryResponse>(url);

                if (result == null)
                {
                    txtHistoryStatus.Text =
                    "No response received from the AVL Tree API.";
                    return;
                }

                dgHistoryReadings.ItemsSource = result.Readings;

                txtHistoryStatus.Text =
                $"AVL Tree search completed. " +
                $"{result.TotalReadings} reading(s) found for {deviceId}.";
            }
            catch (Exception ex)
            {
                txtHistoryStatus.Text =
                $"Unable to retrieve historical telemetry: {ex.Message}";
            }
            finally
            {
                btnSearchHistory.IsEnabled = true;
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

