namespace SmartX.Api.Models
{
 
    // Represents one level in the Smart-X deployment hierarchy.
    
    // A node may contain child nodes, allowing structures such as:
    // Facility A -> Zone 1 -> Node 1.
   
    public class DeploymentNode
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public List<DeploymentNode> Children { get; set; } = new();
    }
}





