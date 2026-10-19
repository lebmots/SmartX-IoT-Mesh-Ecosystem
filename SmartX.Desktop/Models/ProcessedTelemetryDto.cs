namespace SmartX.Desktop.Models
{
    public class ProcessedTelemetryDto
    {
        public Guid MessageId { get; set; }
        public string DeviceId { get; set; } = "";
        public string Metric { get; set; } = "";
        public double Value { get; set; }
        public string Unit { get; set; } = "";
        public DateTime ReceivedAt { get; set; }
        public bool IsCritical { get; set; }
    }
}

