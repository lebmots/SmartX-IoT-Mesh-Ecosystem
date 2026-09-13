using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    // Evaluates the connectivity status of IoT devices based on the
    // time elapsed since their most recent telemetry transmission.
   
    // IoT monitoring research identifies sensor failures,
    // communication interruptions and missing telemetry as important
    // indicators of device-health problems.
    
    // Reference:
    // Majib, Y. et al. (2023) 'Detecting anomalies within smart buildings
    // using do-it-yourself internet of things',
    // Journal of Ambient Intelligence and Humanized Computing, 14,
    // pp. 4727-4743.

    public class DeviceHealthService
    {
        // Calculates device health based on the LastSeen timestamp.
 
        public DeviceHealthStatus CalculateHealth(DateTime lastSeen)
        {
            TimeSpan timeSinceLastSeen = DateTime.UtcNow - lastSeen;

            if (timeSinceLastSeen.TotalSeconds <= 15)
            {
                return DeviceHealthStatus.Healthy;
            }

            if (timeSinceLastSeen.TotalSeconds <= 30)
            {
                return DeviceHealthStatus.Warning;
            }

            if (timeSinceLastSeen.TotalSeconds <= 60)
            {
                return DeviceHealthStatus.Critical;
            }

            return DeviceHealthStatus.Disconnected;
        }

        public string GetHealthMessage(DateTime lastSeen)
        {
            TimeSpan timeSinceLastSeen = DateTime.UtcNow - lastSeen;

            if (timeSinceLastSeen.TotalSeconds <= 15)
            {
                return "Device is transmitting normally.";
            }

            if (timeSinceLastSeen.TotalSeconds <= 30)
            {
                return "Telemetry transmission is delayed.";
            }

            if (timeSinceLastSeen.TotalSeconds <= 60)
            {
                return "Device communication appears unstable.";
            }

            return "No telemetry has been received for more than 60 seconds.";
        }
    }
}





