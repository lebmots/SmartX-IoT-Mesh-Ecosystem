namespace SmartX.Api.Models
{
    
    // Represents the current operational health of a Smart-X IoT device.
    
    // Device-health indicators allow operators to identify connectivity
    // failures and abnormal telemetry without manually analysing every
    // sensor reading.
   
    // Research:
    // Majib et al. (2023) discuss IoT failures caused by sensor
    // malfunction, communication errors and device power interruptions.
    
    // Reference:
    // Majib, Y. et al. (2023) 'Detecting anomalies within smart buildings
    // using do-it-yourself internet of things',
    // Journal of Ambient Intelligence and Humanized Computing, 14,
    // pp. 4727-4743.
    
    public enum DeviceHealthStatus
    {
        Healthy,
        Warning,
        Critical,
        Disconnected
    }
}




