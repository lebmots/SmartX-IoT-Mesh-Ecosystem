

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

        public string CategoryText
        {
            get
            {
                return Category switch
                {
                    0 => "Environmental",
                    1 => "Power Consumption",
                    2 => "Actuator",
                    _ => "Unknown"
                };
            }
        }

        public string HealthStatusText
        {
            get
            {
                return HealthStatus switch
                {
                    0 => "🟢 Healthy",
                    1 => "🟡 Warning",
                    2 => "🔴 Critical",
                    3 => "⚫ Disconnected",
                    _ => "Unknown"
                };
            }
        }
    }
}







