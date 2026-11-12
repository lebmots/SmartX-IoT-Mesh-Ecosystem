
using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class MeshGraphService
    {
        private readonly Dictionary<string, MeshNode> _nodes =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<MeshEdge>> _adjacency =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        public bool AddNode(MeshNode node)
        {
            if (string.IsNullOrWhiteSpace(node.NodeId))
                return false;

            lock (_lock)
            {
                if (_nodes.ContainsKey(node.NodeId))
                    return false;

                _nodes[node.NodeId] = node;
                _adjacency[node.NodeId] = new List<MeshEdge>();

                return true;
            }
        }

        public bool AddConnection(
        string sourceId,
        string destinationId,
        double weight)
        {
            lock (_lock)
            {
                if (!_nodes.ContainsKey(sourceId) ||
                !_nodes.ContainsKey(destinationId) ||
                string.Equals(sourceId, destinationId,
                StringComparison.OrdinalIgnoreCase) ||
                double.IsNaN(weight) ||
                double.IsInfinity(weight) ||
                weight <= 0)
                {
                    return false;
                }

                if (_adjacency[sourceId].Any(e =>
                string.Equals(e.DestinationId, destinationId,
                StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                // Undirected mesh connection
                _adjacency[sourceId].Add(new MeshEdge
                {
                    SourceId = sourceId,
                    DestinationId = destinationId,
                    Weight = weight
                });

                _adjacency[destinationId].Add(new MeshEdge
                {
                    SourceId = destinationId,
                    DestinationId = sourceId,
                    Weight = weight
                });

                return true;
            }
        }
        public void SeedDemoNetwork()
        {
            AddNode(new MeshNode
            {
                NodeId = "GATEWAY-001",
                MacAddress = "00:00:00:00:00:01",
                IsGateway = true
            });

            for (int i = 1; i <= 4; i++)
            {
                AddNode(new MeshNode
                {
                    NodeId = $"ESP32-000{i}",
                    MacAddress = $"AA:BB:CC:11:22:{i:D2}",
                    IsOnline = true
                });
            }

            // Simulated wireless connections and relative transmission costs
            AddConnection("GATEWAY-001", "ESP32-0001", 2);
            AddConnection("GATEWAY-001", "ESP32-0002", 4);
            AddConnection("ESP32-0001", "ESP32-0002", 1);
            AddConnection("ESP32-0001", "ESP32-0003", 5);
            AddConnection("ESP32-0002", "ESP32-0003", 2);
            AddConnection("ESP32-0002", "ESP32-0004", 6);
            AddConnection("ESP32-0003", "ESP32-0004", 3);
        }






        public object GetTopology()
        {
            lock (_lock)
            {
                return new
                {
                    Nodes = _nodes.Values.ToList(),

                    Connections = _adjacency.Values
                .SelectMany(edges => edges)
                .Where(edge =>
                string.Compare(
                edge.SourceId,
                edge.DestinationId,
                StringComparison.OrdinalIgnoreCase) < 0)
                .ToList(),

                    TotalNodes = _nodes.Count,
                    TotalConnections =
                _adjacency.Values.Sum(edges => edges.Count) / 2
                };
            }
        }
    }
}

