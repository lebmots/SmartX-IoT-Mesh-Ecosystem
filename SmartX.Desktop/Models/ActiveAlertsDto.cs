

namespace SmartX.Desktop.Models
{
    public class ActiveAlertsDto
    {
        public int TotalActiveAlerts { get; set; }
        public List<string> Alerts { get; set; } = new();
    }
}


