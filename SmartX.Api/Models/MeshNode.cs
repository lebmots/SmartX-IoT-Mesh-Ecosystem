namespace SmartX.Api.Models
{
    public class MeshNode
    {
        public string NodeId { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = true;
        public bool IsGateway { get; set; } = false;
    }
}
