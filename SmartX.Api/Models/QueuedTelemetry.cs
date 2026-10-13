namespace SmartX.Api.Models
{
    public class QueuedTelemetry
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string DeviceId { get; set; } = "";
        public string Metric { get; set; } = "";
        public double Value { get; set; }
        public string Unit { get; set; } = "";
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public bool IsCritical { get; set; }
    }
}
