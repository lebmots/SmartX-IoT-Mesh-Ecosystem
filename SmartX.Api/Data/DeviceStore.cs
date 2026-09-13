using SmartX.Api.Models;

namespace SmartX.Api.Data
{

    // In-memory device store used.
    
    public static class DeviceStore
    {
        public static List<SensorDevice> Devices { get; } = new()
{
new SensorDevice
{
Id = 1,
DeviceId = "ESP32-0001",
MacAddress = "AA:BB:CC:11:22:01",
DeploymentLocation = "Facility A / Zone 1 / Node 1",
Category = SensorCategory.Environmental,
LastSeen = DateTime.UtcNow.AddSeconds(-5)
},

new SensorDevice
{
Id = 2,
DeviceId = "ESP32-0002",
MacAddress = "AA:BB:CC:11:22:02",
DeploymentLocation = "Facility A / Zone 1 / Node 2",
Category = SensorCategory.PowerConsumption,
LastSeen = DateTime.UtcNow.AddSeconds(-22)
},

new SensorDevice
{
Id = 3,
DeviceId = "ESP32-0003",
MacAddress = "AA:BB:CC:11:22:03",
DeploymentLocation = "Facility A / Zone 2 / Node 1",
Category = SensorCategory.Actuator,
LastSeen = DateTime.UtcNow.AddSeconds(-42)
},

new SensorDevice
{
Id = 4,
DeviceId = "ESP32-0004",
MacAddress = "AA:BB:CC:11:22:04",
DeploymentLocation = "Facility B / Zone 1 / Node 1",
Category = SensorCategory.Environmental,
LastSeen = DateTime.UtcNow.AddSeconds(-90)
}
};
    }
}



