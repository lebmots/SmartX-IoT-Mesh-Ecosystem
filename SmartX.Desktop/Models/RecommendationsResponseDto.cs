

namespace SmartX.Desktop.Models
{
    public class RecommendationsResponseDto
    {
        public int TotalRecommendations { get; set; }

        public List<SuggestedActionDto> Suggestions { get; set; } = new();
    }
}

