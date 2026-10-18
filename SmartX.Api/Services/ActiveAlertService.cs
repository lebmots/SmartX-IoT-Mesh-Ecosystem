using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class ActiveAlertService
    {
        private readonly HashSet<string> _activeAlerts =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        private static string GetAlertKey(string deviceId, string metric)
        {
            return $"{deviceId.Trim()}|{metric.Trim()}";
        }

        public bool AddAlert(string deviceId, string metric)
        {
            lock (_lock)
            {
                return _activeAlerts.Add(GetAlertKey(deviceId, metric));
            }
        }

        public bool RemoveAlert(string deviceId, string metric)
        {
            lock (_lock)
            {
                return _activeAlerts.Remove(GetAlertKey(deviceId, metric));
            }
        }

        public List<string> GetActiveAlerts()
        {
            lock (_lock)
            {
                return _activeAlerts.OrderBy(a => a).ToList();
            }
        }

        public int GetActiveAlertCount()
        {
            lock (_lock)
            {
                return _activeAlerts.Count;
            }
        }

        public bool ContainsAlert(string deviceId, string metric)
        {
            lock (_lock)
            {
                return _activeAlerts.Contains(GetAlertKey(deviceId, metric));
            }
        }

        public void UpdateFromTelemetry(QueuedTelemetry message)
        {
            if (message.IsCritical)
            {
                AddAlert(message.DeviceId, message.Metric);
            }
            else
            {
                RemoveAlert(message.DeviceId, message.Metric);
            }
        }
    }
}

