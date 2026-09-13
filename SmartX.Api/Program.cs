
using SmartX.Api.Data;
using SmartX.Api.Models;
using SmartX.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DeviceHealthService>();
builder.Services.AddSingleton<TelemetryValidationService>();
builder.Services.AddSingleton<MockTelemetryService>();
builder.Services.AddSingleton<DeploymentHierarchyService>();
builder.Services.AddSingleton<DeviceRegistrationService>();
builder.Services.AddSingleton<AttachmentService>();






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





app.MapGet("/api/operator-demo", () =>
{
    SensorReading meter1 = new()
    {
        DeviceId = "ESP32-0002",
        Metric = "PowerWattage",
        Value = 1200,
        Unit = "W"
    };

    SensorReading meter2 = new()
    {
        DeviceId = "ESP32-0005",
        Metric = "PowerWattage",
        Value = 850,
        Unit = "W"
    };

    SensorReading combinedReading = meter1 + meter2;
    SensorReading difference = meter1 - meter2;

    bool meter1Higher = meter1 > meter2;
    bool meter1Lower = meter1 < meter2;

    return Results.Ok(new
    {
        Meter1 = meter1.Value,
        Meter2 = meter2.Value,
        Combined = combinedReading.Value,
        Difference = difference.Value,
        Meter1HigherThanMeter2 = meter1Higher,
        Meter1LowerThanMeter2 = meter1Lower
    });
});




app.MapGet(
"/api/deployment/demo",
(DeploymentHierarchyService hierarchyService) =>
{
    DeploymentNode facility = new()
    {
        Name = "Facility A",
        Type = "Facility",
        Children =
{
new DeploymentNode
{
Name = "Zone 1",
Type = "Zone",
Children =
{
new DeploymentNode
{
Name = "Node 1",
Type = "Node"
},
new DeploymentNode
{
Name = "Node 2",
Type = "Node"
}
}
},
new DeploymentNode
{
Name = "Zone 2",
Type = "Zone",
Children =
{
new DeploymentNode
{
Name = "Node 3",
Type = "Node"
}
}
}
}
    };

    bool isValid =
    hierarchyService.ValidateHierarchy(
    facility,
    out string validationMessage);

    int totalNodes =
    hierarchyService.CountNodes(facility);

    return Results.Ok(new
    {
        Hierarchy = facility,
        IsValid = isValid,
        ValidationMessage = validationMessage,
        TotalNodes = totalNodes
    });
});


app.MapGet(
"/api/deployment/invalid-demo",
(DeploymentHierarchyService hierarchyService) =>
{
    DeploymentNode facility = new()
    {
        Name = "Facility B",
        Type = "Facility",
        Children =
{
new DeploymentNode
{
Name = "Zone 1",
Type = "Zone",
Children =
{
new DeploymentNode
{
Name = "",
Type = "Node"
}
}
}
}
    };

    bool isValid =
    hierarchyService.ValidateHierarchy(
    facility,
    out string validationMessage);

    return Results.Ok(new
    {
        IsValid = isValid,
        ValidationMessage = validationMessage
    });
});





app.MapPost(
"/api/devices/register",
(
DeviceRegistrationRequest request,
DeviceRegistrationService registrationService
) =>
{
    bool registered =
    registrationService.RegisterDevice(
    request,
    out SensorDevice? device,
    out string message);

    if (!registered)
    {
        return Results.BadRequest(new
        {
            error = message
        });
    }

    return Results.Created(
    $"/api/devices/{device!.Id}",
    new
    {
        message,
        device
    });
});






app.MapGet(
"/api/devices/{id:int}",
(int id) =>
{
    SensorDevice? device =
    DeviceStore.Devices.FirstOrDefault(
    d => d.Id == id);

    if (device == null)
    {
        return Results.NotFound(new
        {
            error = "Device not found."
        });
    }

    return Results.Ok(device);
});







app.MapPost(
"/api/devices/{deviceId}/attachments",
async (
string deviceId,
HttpRequest request,
AttachmentService attachmentService) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest(new
        {
            error = "Multipart form data is required."
        });
    }

    IFormCollection form =
    await request.ReadFormAsync();

    IFormFile? file =
    form.Files.GetFile("file");

    string attachmentCategory =
    form["attachmentCategory"].ToString();

    string uploadRoot =
    Path.Combine(
    Directory.GetCurrentDirectory(),
    "Uploads");

    var result =
    await attachmentService.SaveAttachmentAsync(
    deviceId,
    attachmentCategory,
    file!,
    uploadRoot);

    if (!result.Success)
    {
        return Results.BadRequest(new
        {
            error = result.Message
        });
    }

    return Results.Ok(new
    {
        message = result.Message,
        attachment = result.Attachment
    });
});




app.MapGet(
"/api/devices/{deviceId}/attachments",
(string deviceId) =>
{
    var attachments =
    AttachmentStore.Attachments
    .Where(
    a => a.DeviceId.Equals(
    deviceId,
    StringComparison.OrdinalIgnoreCase))
    .ToList();

    return Results.Ok(attachments);
});




app.Run();











