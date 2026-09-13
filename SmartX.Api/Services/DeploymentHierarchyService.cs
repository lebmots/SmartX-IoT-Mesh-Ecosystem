

using SmartX.Api.Models;

namespace SmartX.Api.Services
{
   
    // Validates nested Smart-X deployment structures recursively.
 
    public class DeploymentHierarchyService
    {
     
        public bool ValidateHierarchy(
        DeploymentNode node,
        out string message)
        {
            if (string.IsNullOrWhiteSpace(node.Name))
            {
                message = "Every deployment node must have a name.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(node.Type))
            {
                message =
                $"Deployment node '{node.Name}' must have a type.";

                return false;
            }

            foreach (DeploymentNode child in node.Children)
            {
                bool childIsValid =
                ValidateHierarchy(child, out string childMessage);

                if (!childIsValid)
                {
                    message = childMessage;
                    return false;
                }
            }

            message = "Deployment hierarchy is valid.";
            return true;
        }

      
        public int CountNodes(DeploymentNode node)
        {
            int count = 1;

            foreach (DeploymentNode child in node.Children)
            {
                count += CountNodes(child);
            }

            return count;
        }
    }
}





