namespace SmartX.Api.Models
{
 
    // Represents the information required to register
    // a new IoT sensor device with the Smart-X gateway.
    
    public class DeviceRegistrationRequest
    {
        public string DeviceId { get; set; } = string.Empty;

        public string MacAddress { get; set; } = string.Empty;

        public string DeploymentLocation { get; set; } = string.Empty;

        public SensorCategory Category { get; set; }
    }
}



