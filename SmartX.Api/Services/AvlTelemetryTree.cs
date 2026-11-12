using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class AvlTelemetryTree
    {
        private class Node
        {
            public DateTime Timestamp { get; set; }
            public List<QueuedTelemetry> Readings { get; } = new();
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int Height { get; set; } = 1;

            public Node(QueuedTelemetry reading)
            {
                Timestamp = reading.ReceivedAt;
                Readings.Add(reading);
            }
        }

        private Node? _root;
        private readonly object _lock = new();

        private static int Height(Node? node) => node?.Height ?? 0;

        private static void UpdateHeight(Node node)
        {
            node.Height = 1 + Math.Max(
            Height(node.Left),
            Height(node.Right));
        }

        private static int Balance(Node node)
        {
            return Height(node.Left) - Height(node.Right);
        }

        private static Node RotateRight(Node root)
        {
            Node newRoot = root.Left!;
            root.Left = newRoot.Right;
            newRoot.Right = root;

            UpdateHeight(root);
            UpdateHeight(newRoot);

            return newRoot;
        }

        private static Node RotateLeft(Node root)
        {
            Node newRoot = root.Right!;
            root.Right = newRoot.Left;
            newRoot.Left = root;

            UpdateHeight(root);
            UpdateHeight(newRoot);

            return newRoot;
        }

        private static Node Insert(
        Node? node,
        QueuedTelemetry reading)
        {
            if (node == null)
                return new Node(reading);

            int comparison =
            reading.ReceivedAt.CompareTo(node.Timestamp);

            if (comparison < 0)
                node.Left = Insert(node.Left, reading);
            else if (comparison > 0)
                node.Right = Insert(node.Right, reading);
            else
            {
                node.Readings.Add(reading);
                return node;
            }

            UpdateHeight(node);

            int balance = Balance(node);

            if (balance > 1)
            {
                if (reading.ReceivedAt > node.Left!.Timestamp)
                    node.Left = RotateLeft(node.Left);

                return RotateRight(node);
            }

            if (balance < -1)
            {
                if (reading.ReceivedAt < node.Right!.Timestamp)
                    node.Right = RotateRight(node.Right);

                return RotateLeft(node);
            }

            return node;
        }

        public void Add(QueuedTelemetry reading)
        {
            lock (_lock)
            {
                _root = Insert(_root, reading);
            }
        }

        public List<QueuedTelemetry> SearchRange(
        DateTime start,
        DateTime end)
        {
            var results = new List<QueuedTelemetry>();

            lock (_lock)
            {
                Search(_root, start, end, results);
            }

            return results;
        }

        private static void Search(
        Node? node,
        DateTime start,
        DateTime end,
        List<QueuedTelemetry> results)
        {
            if (node == null)
                return;

            if (start < node.Timestamp)
                Search(node.Left, start, end, results);

            if (node.Timestamp >= start &&
            node.Timestamp <= end)
            {
                results.AddRange(node.Readings);
            }

            if (end > node.Timestamp)
                Search(node.Right, start, end, results);
        }
    }
}

