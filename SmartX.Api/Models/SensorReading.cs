namespace SmartX.Api.Models
{
   
    // Represents a numerical sensor reading that can participate
    // in arithmetic and comparison operations.
    
    // Operator overloading allows Smart-X to combine or compare
    // compatible sensor readings using natural C# syntax.
    
    // Example:
    // SensorReading combined = meter1 + meter2;
    
    // Reference:
    // Microsoft (2026) Operator overloading - C# reference.
    // Available at:
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading
    
    public class SensorReading
    {
        public string DeviceId { get; set; } = string.Empty;

        public string Metric { get; set; } = string.Empty;

        public double Value { get; set; }

        public string Unit { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

 
        // Adds two compatible sensor readings.
      
        public static SensorReading operator +(
        SensorReading first,
        SensorReading second)
        {
            ValidateCompatibility(first, second);

            return new SensorReading
            {
                DeviceId = $"{first.DeviceId}+{second.DeviceId}",
                Metric = first.Metric,
                Value = first.Value + second.Value,
                Unit = first.Unit,
                Timestamp = DateTime.UtcNow
            };
        }

        // Calculates the difference between two compatible readings.
      
        public static SensorReading operator -(
        SensorReading first,
        SensorReading second)
        {
            ValidateCompatibility(first, second);

            return new SensorReading
            {
                DeviceId = $"{first.DeviceId}-{second.DeviceId}",
                Metric = first.Metric,
                Value = first.Value - second.Value,
                Unit = first.Unit,
                Timestamp = DateTime.UtcNow
            };
        }


        // Determines whether the first reading is greater than the second.
     
        public static bool operator >(
        SensorReading first,
        SensorReading second)
        {
            ValidateCompatibility(first, second);
            return first.Value > second.Value;
        }

        
        // Determines whether the first reading is less than the second.
        
        public static bool operator <(
        SensorReading first,
        SensorReading second)
        {
            ValidateCompatibility(first, second);
            return first.Value < second.Value;
        }

        
        // Prevents invalid operations between unrelated metrics or units.
       
        private static void ValidateCompatibility(
        SensorReading first,
        SensorReading second)
        {
            if (!first.Metric.Equals(
            second.Metric,
            StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                "Sensor readings must use the same metric.");
            }

            if (!first.Unit.Equals(
            second.Unit,
            StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                "Sensor readings must use the same unit.");
            }
        }
    }
}






