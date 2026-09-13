
using SmartX.Api.Data;
using SmartX.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DeviceHealthService>();

var app = builder.Build();

TelemetrySeedData.Seed();

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

app.Run();











