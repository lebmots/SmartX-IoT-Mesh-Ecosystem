

namespace SmartX.Desktop.Models
{
    public class QueueStatusDto
    {
        public int NormalQueued { get; set; }
        public int CriticalQueued { get; set; }
        public int TotalQueued { get; set; }
        public int TotalProcessed { get; set; }
    }
}


