using SmartX.Api.Models;

namespace SmartX.Api.Data
{
 
    // Seeds representative Smart-X telemetry for testing.
 
    public static class TelemetrySeedData
    {
        public static void Seed()
        {
            if (TelemetryStore.FloatTelemetry.Count > 0 ||
            TelemetryStore.IntegerTelemetry.Count > 0 ||
            TelemetryStore.BooleanTelemetry.Count > 0)
            {
                return;
            }

            // Float telemetry:
            // Represents an environmental reading such as temperature.
            TelemetryStore.FloatTelemetry.Add(
            new TelemetryPacket<float>
            {
                DeviceId = "ESP32-0001",
                Metric = "Temperature",
                Value = 24.7f,
                Unit = "°C",
                Timestamp = DateTime.UtcNow
            });

            // Another float example for soil moisture.
            TelemetryStore.FloatTelemetry.Add(
            new TelemetryPacket<float>
            {
                DeviceId = "ESP32-0001",
                Metric = "SoilMoisture",
                Value = 63.5f,
                Unit = "%",
                Timestamp = DateTime.UtcNow
            });

            // Integer telemetry:
            // Represents electrical power consumption in watts.
            TelemetryStore.IntegerTelemetry.Add(
            new TelemetryPacket<int>
            {
                DeviceId = "ESP32-0002",
                Metric = "PowerWattage",
                Value = 1842,
                Unit = "W",
                Timestamp = DateTime.UtcNow
            });

            // Boolean telemetry:
            // Represents the state of an IoT actuator such as a valve.
            TelemetryStore.BooleanTelemetry.Add(
            new TelemetryPacket<bool>
            {
                DeviceId = "ESP32-0003",
                Metric = "ValveState",
                Value = true,
                Unit = "State",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


