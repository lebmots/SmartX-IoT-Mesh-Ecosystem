
namespace SmartX.Api.Models
{
    public class DeviceCommandRecord
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();
        public string DeviceId { get; set; } = "";
        public string CommandType { get; set; } = "";
        public bool PreviousState { get; set; }
        public bool NewState { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public bool WasUndone { get; set; }
    }
}


