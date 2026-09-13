
using SmartX.Api.Data;
using SmartX.Api.Models;
using SmartX.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DeviceHealthService>();
builder.Services.AddSingleton<TelemetryValidationService>();
builder.Services.AddSingleton<MockTelemetryService>();


var app = builder.Build();

TelemetrySeedData.Seed();
var mockTelemetryService =
app.Services.GetRequiredService<MockTelemetryService>();

mockTelemetryService.LoadHistoricalTemperatureBatches();
mockTelemetryService.SeedLargeTelemetrySet();


app.MapGet("/", () =>
{
    return "Smart-X IoT Mesh Gateway is running.";
});

app.MapGet("/api/devices", (DeviceHealthService healthService) =>
{
    foreach (var device in DeviceStore.Devices)
    {
        device.HealthStatus =
        healthService.CalculateHealth(device.LastSeen);

        device.HealthMessage =
        healthService.GetHealthMessage(device.LastSeen);
    }

    return Results.Ok(DeviceStore.Devices);
});

app.MapGet("/api/telemetry/float", () =>
{
    return Results.Ok(TelemetryStore.FloatTelemetry);
});

app.MapGet("/api/telemetry/int", () =>
{
    return Results.Ok(TelemetryStore.IntegerTelemetry);
});

app.MapGet("/api/telemetry/bool", () =>
{
    return Results.Ok(TelemetryStore.BooleanTelemetry);
});




app.MapPost(
"/api/telemetry/float",
(
TelemetryPacket<float> packet,
TelemetryValidationService validationService
) =>
{
    if (!validationService.IsValid(packet, out string message))
    {
        return Results.BadRequest(new
        {
            error = message
        });
    }

    packet.Timestamp = DateTime.UtcNow;

    TelemetryStore.FloatTelemetry.Add(packet);

    SensorDevice? device =
    DeviceStore.Devices.FirstOrDefault(
    d => d.DeviceId.Equals(
    packet.DeviceId,
    StringComparison.OrdinalIgnoreCase));

    if (device != null)
    {
        device.LastSeen = DateTime.UtcNow;
    }

    return Results.Ok(new
    {
        message = "Float telemetry received successfully.",
        packet
    });
});




app.MapPost(
"/api/telemetry/int",
(
TelemetryPacket<int> packet,
TelemetryValidationService validationService
) =>
{
    if (!validationService.IsValid(packet, out string message))
    {
        return Results.BadRequest(new
        {
            error = message
        });
    }

    packet.Timestamp = DateTime.UtcNow;

    TelemetryStore.IntegerTelemetry.Add(packet);

    SensorDevice? device =
    DeviceStore.Devices.FirstOrDefault(
    d => d.DeviceId.Equals(
    packet.DeviceId,
    StringComparison.OrdinalIgnoreCase));

    if (device != null)
    {
        device.LastSeen = DateTime.UtcNow;
    }

    return Results.Ok(new
    {
        message = "Integer telemetry received successfully.",
        packet
    });
});




app.MapPost(
"/api/telemetry/bool",
(
TelemetryPacket<bool> packet,
TelemetryValidationService validationService
) =>
{
    if (!validationService.IsValid(packet, out string message))
    {
        return Results.BadRequest(new
        {
            error = message
        });
    }

    packet.Timestamp = DateTime.UtcNow;

    TelemetryStore.BooleanTelemetry.Add(packet);

    SensorDevice? device =
    DeviceStore.Devices.FirstOrDefault(
    d => d.DeviceId.Equals(
    packet.DeviceId,
    StringComparison.OrdinalIgnoreCase));

    if (device != null)
    {
        device.LastSeen = DateTime.UtcNow;
    }

    return Results.Ok(new
    {
        message = "Boolean telemetry received successfully.",
        packet
    });
});





app.MapGet("/api/telemetry/summary", () =>
{
    return Results.Ok(new
    {
        FloatReadings = TelemetryStore.FloatTelemetry.Count,
        IntegerReadings = TelemetryStore.IntegerTelemetry.Count,
        BooleanReadings = TelemetryStore.BooleanTelemetry.Count,
        TotalReadings =
    TelemetryStore.FloatTelemetry.Count +
    TelemetryStore.IntegerTelemetry.Count +
    TelemetryStore.BooleanTelemetry.Count,
        HistoricalBatchCount =
    HistoricalBatchStore.TemperatureBatches.Length
    });
});








app.Run();











