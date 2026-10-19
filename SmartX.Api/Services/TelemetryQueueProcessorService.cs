namespace SmartX.Api.Services
{
    public class TelemetryQueueProcessorService : BackgroundService
    {
        private readonly TelemetryQueueService _queueService;
        private readonly ILogger<TelemetryQueueProcessorService> _logger;

        public TelemetryQueueProcessorService(
        TelemetryQueueService queueService,
        ILogger<TelemetryQueueProcessorService> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(
            TimeSpan.FromSeconds(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var message = _queueService.ProcessNext();

                if (message != null)
                {
                    _logger.LogInformation(
                    "Processed {Metric} telemetry from {DeviceId}",
                    message.Metric,
                    message.DeviceId);
                }
            }
        }
    }
}


