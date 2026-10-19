using SmartX.Api.Models;
using SmartX.Api.Data;

namespace SmartX.Api.Services
{
    public class RegisteredDeviceLookupService
    {
        private readonly Dictionary<string, SensorDevice> _devices =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        public RegisteredDeviceLookupService()
        {
            foreach (var device in DeviceStore.Devices)
            {
                _devices[device.DeviceId] = device;
            }
        }

        public SensorDevice? FindDevice(string deviceId)
        {
            lock (_lock)
            {
                return _devices.TryGetValue(deviceId, out var device)
                ? device
                : null;
            }
        }

        public void RegisterDevice(SensorDevice device)
        {
            lock (_lock)
            {
                _devices[device.DeviceId] = device;
            }
        }

        public int GetRegisteredCount()
        {
            lock (_lock)
            {
                return _devices.Count;
            }
        }
    }
}



