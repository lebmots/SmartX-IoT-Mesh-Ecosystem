namespace SmartX.Api.Data
{
    // Stores historical telemetry batches before they are transferred
    // into the main List<T> based telemetry collections.
   
    // A jagged array is used because different batches may contain
    // different numbers of readings.
    
    // Reference:
    // Microsoft (2026) Arrays - C# reference.
    // Available at:
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/arrays
    
    public static class HistoricalBatchStore
    {
   
        public static float[][] TemperatureBatches { get; } =
        {
new float[] { 22.4f, 22.8f, 23.1f, 23.5f },
new float[] { 23.7f, 24.0f, 24.3f },
new float[] { 24.6f, 24.9f, 25.1f, 25.4f, 25.7f }
};
    }
}




