using SmartX.Desktop.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;

namespace SmartX.Desktop.Services
{
   
    // Handles communication between the Smart-X WPF client
    // and the ASP.NET Core API.
 
    public class SmartXApiService
    {
        private readonly HttpClient _httpClient;

        public SmartXApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7031/")
            };
        }

 
        // Retrieves all registered Smart-X devices.
   
        public async Task<List<SensorDeviceDto>> GetDevicesAsync()
        {
            List<SensorDeviceDto>? devices =
            await _httpClient.GetFromJsonAsync<List<SensorDeviceDto>>(
            "api/devices");

            return devices ?? new List<SensorDeviceDto>();
        }


        public async Task<List<AnomalyRecordDto>> GetAnomaliesAsync()
        {
            List<AnomalyRecordDto>? anomalies =
            await _httpClient.GetFromJsonAsync<List<AnomalyRecordDto>>(
            "api/anomalies");

            return anomalies ?? new List<AnomalyRecordDto>();
        }

        public async Task<(bool Success, string Message)> RegisterDeviceAsync(
        DeviceRegistrationDto request)
        {
            HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(
            "api/devices/register",
            request);

            if (response.IsSuccessStatusCode)
            {
                return (
                true,
                "Device registered successfully."
                );
            }

            string error =
            await response.Content.ReadAsStringAsync();

            return (
            false,
            error
            );
        }

        public async Task<(bool Success, string Message)> UploadAttachmentAsync(
        string deviceId,
        string filePath,
        string attachmentCategory)
        {
            using MultipartFormDataContent content = new();

            using FileStream fileStream =
            File.OpenRead(filePath);

            using StreamContent fileContent =
            new(fileStream);

            content.Add(
            fileContent,
            "file",
            Path.GetFileName(filePath));

            content.Add(
            new StringContent(attachmentCategory),
            "attachmentCategory");

            HttpResponseMessage response =
            await _httpClient.PostAsync(
            $"api/devices/{deviceId}/attachments",
            content);

            if (response.IsSuccessStatusCode)
            {
                return (
                true,
                "Attachment uploaded successfully."
                );
            }

            string error =
            await response.Content.ReadAsStringAsync();

            return (
            false,
            error
            );
        }

        public async Task<List<CommandCentreDeviceDto>> GetCommandCentreDevicesAsync(
        string search = "",
        string category = "All",
        string alertState = "All")
        {
            string url =
            "api/command-centre/devices" +
            $"?search={Uri.EscapeDataString(search)}" +
            $"&category={Uri.EscapeDataString(category)}" +
            $"&alertState={Uri.EscapeDataString(alertState)}";

            return await _httpClient.GetFromJsonAsync<List<CommandCentreDeviceDto>>(url)
            ?? new List<CommandCentreDeviceDto>();
        }

        public async Task<QueueStatusDto> GetQueueStatusAsync()
        {
            return await _httpClient.GetFromJsonAsync<QueueStatusDto>(
            "api/queue/status") ?? new QueueStatusDto();
        }

        public async Task<List<ProcessedTelemetryDto>> GetProcessedTelemetryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProcessedTelemetryDto>>(
            "api/queue/processed") ?? new List<ProcessedTelemetryDto>();
        }

        public async Task<ActiveAlertsDto> GetActiveAlertsAsync()
        {
            return await _httpClient.GetFromJsonAsync<ActiveAlertsDto>(
            "api/alerts/active") ?? new ActiveAlertsDto();
        }


        public async Task<DeviceCommandRecordDto> ExecuteDeviceCommandAsync(
        DeviceCommandRequestDto request)
        {
            using var response = await _httpClient.PostAsJsonAsync(
            "api/commands/execute",
            request);

            response.EnsureSuccessStatusCode();

            return await response.Content
            .ReadFromJsonAsync<DeviceCommandRecordDto>()
            ?? throw new InvalidOperationException(
            "The API returned an empty command response.");
        }

        public async Task<DeviceCommandRecordDto> UndoLastCommandAsync()
        {
            using var response = await _httpClient.PostAsync(
            "api/commands/undo",
            null);

            response.EnsureSuccessStatusCode();

            return await response.Content
            .ReadFromJsonAsync<DeviceCommandRecordDto>()
            ?? throw new InvalidOperationException(
            "The API returned an empty undo response.");
        }

        public async Task<List<DeviceCommandRecordDto>> GetCommandHistoryAsync()
        {
            return await _httpClient
            .GetFromJsonAsync<List<DeviceCommandRecordDto>>(
            "api/commands/history")
            ?? new List<DeviceCommandRecordDto>();
        }

        public async Task<RecommendationsResponseDto> GetRecommendationsAsync()
        {
            return await _httpClient
            .GetFromJsonAsync<RecommendationsResponseDto>(
            "api/recommendations")
            ?? new RecommendationsResponseDto();
        }


    }
}


