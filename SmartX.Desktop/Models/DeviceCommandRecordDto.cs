namespace SmartX.Desktop.Models
{
    public class DeviceCommandRecordDto
    {
        public Guid CommandId { get; set; }
        public string DeviceId { get; set; } = "";
        public string CommandType { get; set; } = "";
        public bool PreviousState { get; set; }
        public bool NewState { get; set; }
        public DateTime ExecutedAt { get; set; }
        public bool WasUndone { get; set; }
    }
}
