
using SmartX.Api.Data;
using SmartX.Api.Models;

namespace SmartX.Api.Services
{
   
    // Handles validation and storage of files uploaded
    // for registered Smart-X IoT devices.
   
    public class AttachmentService
    {
        private static readonly string[] AllowedExtensions =
        {
".txt",
".log",
".json",
".xml",
".csv",
".jpg",
".jpeg",
".png"
};

        public async Task<(bool Success, string Message, DeviceAttachment? Attachment)>
        SaveAttachmentAsync(
        string deviceId,
        string attachmentCategory,
        IFormFile file,
        string uploadRoot)
        {
            SensorDevice? device =
            DeviceStore.Devices.FirstOrDefault(
            d => d.DeviceId.Equals(
            deviceId,
            StringComparison.OrdinalIgnoreCase));

            if (device == null)
            {
                return (
                false,
                "The specified device is not registered.",
                null);
            }

            if (file == null || file.Length == 0)
            {
                return (
                false,
                "A file must be selected.",
                null);
            }

            string extension =
            Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                return (
                false,
                "Unsupported file type.",
                null);
            }

            string[] allowedCategories =
            {
"Configuration",
"DeploymentPhoto",
"HardwareLog"
};

            if (!allowedCategories.Contains(
            attachmentCategory,
            StringComparer.OrdinalIgnoreCase))
            {
                return (
                false,
                "Invalid attachment category.",
                null);
            }

            Directory.CreateDirectory(uploadRoot);

            string storedFileName =
            $"{Guid.NewGuid()}{extension}";

            string fullPath =
            Path.Combine(uploadRoot, storedFileName);

            using FileStream stream =
            new(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);

            int newId =
            AttachmentStore.Attachments.Count == 0
            ? 1
            : AttachmentStore.Attachments.Max(a => a.Id) + 1;

            DeviceAttachment attachment = new()
            {
                Id = newId,
                DeviceId = deviceId,
                OriginalFileName = file.FileName,
                StoredFileName = storedFileName,
                FileType = extension,
                AttachmentCategory = attachmentCategory,
                FilePath = fullPath,
                UploadedAt = DateTime.UtcNow
            };

            AttachmentStore.Attachments.Add(attachment);

            return (
            true,
            "Attachment uploaded successfully.",
            attachment);
        }
    }
}





