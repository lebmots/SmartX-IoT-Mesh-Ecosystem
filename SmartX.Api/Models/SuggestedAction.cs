namespace SmartX.Api.Models
{
    public class SuggestedAction
    {
        public string DeviceId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string RecommendedCommand { get; set; } = "";
        public string Priority { get; set; } = "Normal";
        public string Reason { get; set; } = "";
    }
}
