
namespace SmartX.Desktop.Models
{
    public class CommandCentreDeviceDto
    {
        public string DeviceId { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public string DeploymentLocation { get; set; } = "";
        public string Category { get; set; } = "";
        public string Health { get; set; } = "";
        public bool HasActiveAlert { get; set; }
    }
}
