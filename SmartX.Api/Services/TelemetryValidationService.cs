using SmartX.Api.Data;
using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    // Validates incoming telemetry before it is accepted by the Smart-X gateway.
    
    // IoT gateways should validate incoming device data before processing it
    // so that invalid or unexpected telemetry does not enter the system.
    
    // Reference:
    // Mishra, A., Cohen, A., Reichherzer, T. and Wilde, N. (2021)
    // 'Detection of data anomalies at the edge of pervasive IoT systems',
    // Computing, 103, pp. 1657-1675.
   
    public class TelemetryValidationService
    {
    
        // Checks whether a device exists in the registered Smart-X device list.
        
        public bool DeviceExists(string deviceId)
        {
            return DeviceStore.Devices.Any(
            d => d.DeviceId.Equals(
            deviceId,
            StringComparison.OrdinalIgnoreCase));
        }


        public bool IsValid<T>(TelemetryPacket<T> packet, out string message)
        {
            if (string.IsNullOrWhiteSpace(packet.DeviceId))
            {
                message = "Device ID is required.";
                return false;
            }

            if (!DeviceExists(packet.DeviceId))
            {
                message = "The device is not registered with the Smart-X gateway.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(packet.Metric))
            {
                message = "Telemetry metric is required.";
                return false;
            }

            if (packet.Value is null)
            {
                message = "Telemetry value is required.";
                return false;
            }

            message = "Telemetry packet is valid.";
            return true;
        }
    }
}




