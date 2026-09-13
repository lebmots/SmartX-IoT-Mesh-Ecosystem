namespace SmartX.Api.Models
{
    // Represents a generic telemetry packet received by the Smart-X gateway.

    // Examples:
    // TelemetryPacket<float> -> temperature / soil moisture
    // TelemetryPacket<int> -> power wattage
    // TelemetryPacket<bool> -> valve state
   
    // Reference:
    // Microsoft (2026) Generics in .NET.
    // Available at:
    // https://learn.microsoft.com/en-us/dotnet/standard/generics/
    
    public class TelemetryPacket<T>
    {
        // Identifies the device that generated the telemetry.
  
        public string DeviceId { get; set; } = string.Empty;

        // Describes the measured telemetry type.

        public string Metric { get; set; } = string.Empty;
 
        public T Value { get; set; } = default!;

        // Unit of measurement where applicable.
        // Example: °C, %, W.
 
        public string Unit { get; set; } = string.Empty;

        // Time at which the telemetry reading was generated.
        // UTC is used to keep timestamps consistent.
 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}





