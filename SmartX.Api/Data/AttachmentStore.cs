
using SmartX.Api.Models;

namespace SmartX.Api.Data
{
    
    // In-memory store for uploaded Smart-X device attachments.
    
    public static class AttachmentStore
    {
        public static List<DeviceAttachment> Attachments { get; } = new();
    }
}
