using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class AnomalyDetectionService
    {
        public bool IsFloatAnomaly(
        TelemetryPacket<float> packet,
        out string message)
        {
            message = string.Empty;

            string metric =
            packet.Metric.Trim().ToLower();

            if (metric.Contains("temperature"))
            {
                if (packet.Value < 10 || packet.Value > 40)
                {
                    message =
                    $"Temperature anomaly detected: {packet.Value}°C.";
                    return true;
                }
            }

            if (metric.Contains("moisture"))
            {
                if (packet.Value < 20 || packet.Value > 90)
                {
                    message =
                    $"Soil moisture anomaly detected: {packet.Value}%.";
                    return true;
                }
            }

            return false;
        }

        public bool IsIntegerAnomaly(
        TelemetryPacket<int> packet,
        out string message)
        {
            message = string.Empty;

            string metric =
            packet.Metric.Trim().ToLower();

            if (metric.Contains("watt"))
            {
                if (packet.Value < 0 || packet.Value > 5000)
                {
                    message =
                    $"Power consumption anomaly detected: {packet.Value} W.";
                    return true;
                }
            }

            return false;
        }
    }
}




