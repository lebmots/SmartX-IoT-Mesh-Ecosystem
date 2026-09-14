namespace SmartX.Desktop.Models
{
    public class DeviceRegistrationDto
    {
        public string DeviceId { get; set; } = string.Empty;

        public string MacAddress { get; set; } = string.Empty;

        public string DeploymentLocation { get; set; } = string.Empty;

        public int Category { get; set; }
    }
}



