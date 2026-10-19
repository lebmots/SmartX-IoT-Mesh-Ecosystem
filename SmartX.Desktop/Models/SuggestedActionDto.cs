namespace SmartX.Desktop.Models
{
    public class SuggestedActionDto
    {
        public string DeviceId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string RecommendedCommand { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Reason { get; set; } = "";
    }
}
