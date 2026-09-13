namespace SmartX.Api.Models
{
    // Represents an IoT device registered with the Smart-X gateway.
    
    // Each sensor stores identification, deployment and health information
    // so that the gateway can associate incoming telemetry with a specific
    // physical or simulated ESP32 device.
   
    public class SensorDevice
    {
 
        public int Id { get; set; }

        // Human-readable or externally supplied unique device identifier.
        // Example: ESP32-0001.
      
        public string DeviceId { get; set; } = string.Empty;

        // MAC address associated with the ESP32 device.
      
        public string MacAddress { get; set; } = string.Empty;

        // Physical or logical Smart-X deployment location.
        // Example: Facility A / Zone 1 / Node 3.
   
        public string DeploymentLocation { get; set; } = string.Empty;

        // Identifies the type of sensor or device.

        public SensorCategory Category { get; set; }

        // Timestamp of the most recent telemetry message or heartbeat.
        // This value is used to determine device connectivity.
        
        public DateTime LastSeen { get; set; }

        // Current calculated device-health status.

        public DeviceHealthStatus HealthStatus { get; set; }

        // Provides additional information about the current health state.
        // Example: "No telemetry received for 45 seconds."
 
        public string HealthMessage { get; set; } = string.Empty;
    }
}






