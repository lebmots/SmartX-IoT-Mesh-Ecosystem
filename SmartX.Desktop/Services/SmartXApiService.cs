using System.Net.Http;
using System.Net.Http.Json;
using SmartX.Desktop.Models;

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

    }
}


