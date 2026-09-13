
using SmartX.Api.Data;
using SmartX.Api.Models;

namespace SmartX.Api.Services
{ 
    // Generates simulated IoT telemetry for Smart-X.
    
    public class MockTelemetryService
    {
        private readonly Random _random = new();

        // Generates a large collection of mock telemetry readings.

        public void SeedLargeTelemetrySet(int readingsPerType = 500)
        {
            if (TelemetryStore.FloatTelemetry.Count > 20)
            {
                return;
            }

            for (int i = 0; i < readingsPerType; i++)
            {
                // Environmental float telemetry.
                float temperature =
                18.0f + (float)(_random.NextDouble() * 17.0);

                TelemetryStore.FloatTelemetry.Add(
                new TelemetryPacket<float>
                {
                    DeviceId = "ESP32-0001",
                    Metric = "Temperature",
                    Value = temperature,
                    Unit = "°C",
                    Timestamp = DateTime.UtcNow.AddSeconds(-i)
                });

                // Power-consumption integer telemetry.
                int wattage = _random.Next(500, 3001);

                TelemetryStore.IntegerTelemetry.Add(
                new TelemetryPacket<int>
                {
                    DeviceId = "ESP32-0002",
                    Metric = "PowerWattage",
                    Value = wattage,
                    Unit = "W",
                    Timestamp = DateTime.UtcNow.AddSeconds(-i)
                });

                // Actuator boolean telemetry.
                bool valveState = _random.Next(0, 2) == 1;

                TelemetryStore.BooleanTelemetry.Add(
                new TelemetryPacket<bool>
                {
                    DeviceId = "ESP32-0003",
                    Metric = "ValveState",
                    Value = valveState,
                    Unit = "State",
                    Timestamp = DateTime.UtcNow.AddSeconds(-i)
                });
            }
        }

    
        public void LoadHistoricalTemperatureBatches()
        {
            foreach (float[] batch in HistoricalBatchStore.TemperatureBatches)
            {
                foreach (float reading in batch)
                {
                    TelemetryStore.FloatTelemetry.Add(
                    new TelemetryPacket<float>
                    {
                        DeviceId = "ESP32-0001",
                        Metric = "HistoricalTemperature",
                        Value = reading,
                        Unit = "°C",
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }
    }
}




