using SmartX.Api.Data;

namespace SmartX.Api.Services
{
  
    // Simulates ESP32 connectivity behaviour for the Smart-X assessment.
    // Each device is assigned a different LastSeen delay so the dashboard
    // can demonstrate Healthy, Warning, Critical and Disconnected states.
   
    public class DeviceSimulationService
    {
        public void UpdateSimulatedConnectivity()
        {
            DateTime now = DateTime.UtcNow;

            var device1 = DeviceStore.Devices
            .FirstOrDefault(d => d.DeviceId == "ESP32-0001");

            var device2 = DeviceStore.Devices
            .FirstOrDefault(d => d.DeviceId == "ESP32-0002");

            var device3 = DeviceStore.Devices
            .FirstOrDefault(d => d.DeviceId == "ESP32-0003");

            var device4 = DeviceStore.Devices
            .FirstOrDefault(d => d.DeviceId == "ESP32-0004");

            // Healthy: telemetry received very recently.
            if (device1 != null)
            {
                device1.LastSeen = now.AddSeconds(-5);
            }

            // Warning: delayed telemetry.
            if (device2 != null)
            {
                device2.LastSeen = now.AddSeconds(-22);
            }

            // Critical: unstable or heavily delayed communication.
            if (device3 != null)
            {
                device3.LastSeen = now.AddSeconds(-45);
            }

            // Disconnected: no telemetry for more than 60 seconds.
            if (device4 != null)
            {
                device4.LastSeen = now.AddSeconds(-90);
            }
        }
    }
}

