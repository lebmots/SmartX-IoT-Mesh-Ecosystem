namespace SmartX.Desktop.Models
{
    public class AnomalyRecordDto
    {
        public int Id { get; set; }

        public string DeviceId { get; set; } = string.Empty;

        public string Metric { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; }

        public string DetectedAtText =>
        DetectedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    }
}




