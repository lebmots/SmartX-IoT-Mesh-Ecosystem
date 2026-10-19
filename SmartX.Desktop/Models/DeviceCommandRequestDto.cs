namespace SmartX.Desktop.Models
{
    public class DeviceCommandRequestDto
    {
        public string DeviceId { get; set; } = "";
        public string CommandType { get; set; } = "";
        public bool TurnOn { get; set; }
    }
}
