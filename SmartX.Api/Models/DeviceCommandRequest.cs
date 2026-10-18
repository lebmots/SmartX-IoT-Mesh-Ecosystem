namespace SmartX.Api.Models
{
    public class DeviceCommandRequest
    {
        public string DeviceId { get; set; } = "";
        public string CommandType { get; set; } = "";
        public bool TurnOn { get; set; }
    }
}

