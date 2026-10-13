

using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class TelemetryHistoryService
    {
        private readonly SortedDictionary<DateTime, List<QueuedTelemetry>>
        _history = new();

        private readonly object _lock = new();

        public void AddReading(QueuedTelemetry message)
        {
            lock (_lock)
            {
                if (!_history.TryGetValue(message.ReceivedAt, out var readings))
                {
                    readings = new List<QueuedTelemetry>();
                    _history[message.ReceivedAt] = readings;
                }

                readings.Add(message);
            }
        }

        public List<QueuedTelemetry> GetHistory()
        {
            lock (_lock)
            {
                return _history
                .SelectMany(entry => entry.Value)
                .ToList();
            }
        }
    }
}




