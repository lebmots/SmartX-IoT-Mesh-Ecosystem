using SmartX.Api.Models;

namespace SmartX.Api.Data
{
    // Stores simulated telemetry packets of Smart-X.

    public static class TelemetryStore
    {
        public static List<TelemetryPacket<float>> FloatTelemetry { get; } = new();

        public static List<TelemetryPacket<int>> IntegerTelemetry { get; } = new();

        public static List<TelemetryPacket<bool>> BooleanTelemetry { get; } = new();
    }
}



























