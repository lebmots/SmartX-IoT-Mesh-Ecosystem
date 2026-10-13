
using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class DeviceLookupService
    {
        private readonly Dictionary<string, QueuedTelemetry>
        _latestReadings = new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        public void UpdateDevice(QueuedTelemetry message)
        {
            lock (_lock)
            {
                _latestReadings[message.DeviceId] = message;
            }
        }

        public QueuedTelemetry? FindDevice(string deviceId)
        {
            lock (_lock)
            {
                return _latestReadings.TryGetValue(deviceId, out var reading)
                ? reading
                : null;
            }
        }

        public List<QueuedTelemetry> GetAllDevices()
        {
            lock (_lock)
            {
                return _latestReadings.Values.ToList();
            }
        }
    }
}



