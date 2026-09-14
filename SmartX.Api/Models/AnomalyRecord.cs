

namespace SmartX.Api.Models
{
    public class AnomalyRecord
    {
        public int Id { get; set; }

        public string DeviceId { get; set; }
        = string.Empty;

        public string Metric { get; set; }
        = string.Empty;

        public string Message { get; set; }
        = string.Empty;

        public DateTime DetectedAt { get; set; }
        = DateTime.UtcNow;
    }
}




