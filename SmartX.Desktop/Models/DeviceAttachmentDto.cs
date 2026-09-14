namespace SmartX.Desktop.Models
{
    public class DeviceAttachmentDto
    {
        public int Id { get; set; }

        public string DeviceId { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public string AttachmentCategory { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}




