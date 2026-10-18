using System.Net.Http.Json;
using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class Esp32HttpSimulatorService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Esp32HttpSimulatorService> _logger;
        private readonly IConfiguration _configuration;

        public Esp32HttpSimulatorService(
        IHttpClientFactory httpClientFactory,
        ILogger<Esp32HttpSimulatorService> logger,
        IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
        {
            // Give the API time to start listening.
            await Task.Delay(3000, stoppingToken);

            string apiBaseUrl =
            _configuration["Simulator:ApiBaseUrl"]
            ?? "https://localhost:7031/";

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiBaseUrl);

            string[] devices =
            {
"ESP32-0001",
"ESP32-0002",
"ESP32-0003",
"ESP32-0004"
};

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string deviceId =
                    devices[Random.Shared.Next(devices.Length)];

                    bool criticalReading =
                    Random.Shared.Next(10) == 0;

                    double temperature = criticalReading
                    ? Random.Shared.Next(45, 65)
                    : Random.Shared.Next(18, 35);

                    var packet = new QueuedTelemetry
                    {
                        DeviceId = deviceId,
                        Metric = "Temperature",
                        Value = temperature,
                        Unit = "C"
                    };

                    using var response = await client.PostAsJsonAsync(
                    "api/queue/enqueue",
                    packet,
                    stoppingToken);

                    response.EnsureSuccessStatusCode();

                    _logger.LogInformation(
                    "Simulated ESP32 {DeviceId} sent {Value} C",
                    deviceId,
                    temperature);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning(
                    ex,
                    "ESP32 simulation HTTP request failed.");
                }
                catch (TaskCanceledException)
                when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}





