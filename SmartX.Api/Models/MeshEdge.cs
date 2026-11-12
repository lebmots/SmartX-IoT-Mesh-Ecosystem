namespace SmartX.Api.Models
{
    public class MeshEdge
    {
        public string SourceId { get; set; } = string.Empty;
        public string DestinationId { get; set; } = string.Empty;

        // Lower weights represent more efficient links.
        public double Weight { get; set; }
    }
}