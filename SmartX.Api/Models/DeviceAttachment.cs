namespace SmartX.Api.Models
{
    // Represents a file attached to a registered Smart-X device.
    // Attachments may include configuration files,
    // deployment photographs, or hardware logs.
 
    public class DeviceAttachment
    {
        public int Id { get; set; }

        public string DeviceId { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public string AttachmentCategory { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}




