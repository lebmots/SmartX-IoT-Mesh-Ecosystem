

namespace SmartX.Desktop.Models
{
   
    // Data transfer model used by the WPF client
    // when receiving Smart-X device information from the API.
  
    public class SensorDeviceDto
    {
        public int Id { get; set; }

        public string DeviceId { get; set; } = string.Empty;

        public string MacAddress { get; set; } = string.Empty;

        public string DeploymentLocation { get; set; } = string.Empty;

        public int Category { get; set; }

        public DateTime LastSeen { get; set; }

        public int HealthStatus { get; set; }

        public string HealthMessage { get; set; } = string.Empty;
    }
}




