
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
builder.Services.AddSingleton<DeviceSimulationService>();
builder.Services.AddSingleton<AnomalyDetectionService>();
builder.Services.AddSingleton<TelemetryQueueService>();
builder.Services.AddSingleton<DeviceLookupService>();
builder.Services.AddSingleton<TelemetryHistoryService>();
builder.Services.AddSingleton<ActiveAlertService>();
builder.Services.AddSingleton<DeviceCommandService>();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<Esp32HttpSimulatorService>();
builder.Services.AddSingleton<RecommendationEngineService>();
builder.Services.AddSingleton<OperatorActivityService>();
builder.Services.AddHostedService<TelemetryQueueProcessorService>();


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

app.MapGet(
"/api/devices",
(
DeviceHealthService healthService,
DeviceSimulationService simulationService
) =>
{
    simulationService.UpdateSimulatedConnectivity();

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



app.MapGet("/api/anomalies", () =>
{
    return Results.Ok(
    AnomalyStore.Anomalies
    .OrderByDescending(a => a.DetectedAt)
    .ToList());
});



app.MapPost(
"/api/telemetry/float",
(
TelemetryPacket<float> packet,
TelemetryValidationService validationService,
AnomalyDetectionService anomalyDetectionService
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

    if (anomalyDetectionService.IsFloatAnomaly(
    packet,
    out string anomalyMessage))
    {
        AnomalyStore.Anomalies.Add(
        new AnomalyRecord
        {
            Id = AnomalyStore.Anomalies.Count + 1,
            DeviceId = packet.DeviceId,
            Metric = packet.Metric,
            Message = anomalyMessage,
            DetectedAt = DateTime.UtcNow
        });
    }


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
TelemetryValidationService validationService,
AnomalyDetectionService anomalyDetectionService
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


    if (anomalyDetectionService.IsIntegerAnomaly(
    packet,
    out string anomalyMessage))
    {
        AnomalyStore.Anomalies.Add(
        new AnomalyRecord
        {
            Id = AnomalyStore.Anomalies.Count + 1,
            DeviceId = packet.DeviceId,
            Metric = packet.Metric,
            Message = anomalyMessage,
            DetectedAt = DateTime.UtcNow
        });
    }



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


app.MapPost("/api/queue/enqueue",
(QueuedTelemetry message, TelemetryQueueService queueService, DeviceLookupService deviceLookup, TelemetryHistoryService historyService, ActiveAlertService alertService) =>
{
    if (string.IsNullOrWhiteSpace(message.DeviceId) ||
    string.IsNullOrWhiteSpace(message.Metric) ||
    double.IsNaN(message.Value) ||
    double.IsInfinity(message.Value))
    {
        return Results.BadRequest("Invalid telemetry message.");
    }

    message.MessageId = Guid.NewGuid();
    message.ReceivedAt = DateTime.UtcNow;

    // Determine severity on the server rather than trusting the client.
    var metric = message.Metric.Trim().ToLowerInvariant();

    message.IsCritical =
    (metric == "temperature" && (message.Value < 10 || message.Value > 40)) ||
    (metric == "soilmoisture" && (message.Value < 20 || message.Value > 90)) ||
    (metric == "powerwattage" && (message.Value < 0 || message.Value > 5000));

    queueService.Enqueue(message);
    deviceLookup.UpdateDevice(message);
    historyService.AddReading(message);
    alertService.UpdateFromTelemetry(message);




    return Results.Ok(new
    {
        message.MessageId,
        message.DeviceId,
        message.IsCritical,
        Status = "Queued"
    });
});

app.MapPost("/api/queue/process-next",
(TelemetryQueueService queueService) =>
{
    var processed = queueService.ProcessNext();

    return processed == null
    ? Results.NoContent()
    : Results.Ok(processed);
});

app.MapGet("/api/queue/status",
(TelemetryQueueService queueService) =>
Results.Ok(queueService.GetQueueStatus()));

app.MapGet("/api/queue/processed",
(TelemetryQueueService queueService) =>
Results.Ok(queueService.GetProcessedMessages()));


app.MapGet("/api/queue/devices",
(DeviceLookupService deviceLookup) =>
Results.Ok(deviceLookup.GetAllDevices()));

app.MapGet("/api/queue/devices/{deviceId}",
(string deviceId, DeviceLookupService deviceLookup) =>
{
    var device = deviceLookup.FindDevice(deviceId);

    return device == null
    ? Results.NotFound("No telemetry found for this device.")
    : Results.Ok(device);
});

app.MapGet("/api/queue/history",
(TelemetryHistoryService historyService) =>
Results.Ok(historyService.GetHistory()));





app.MapGet("/api/alerts/active",
(ActiveAlertService alertService) =>
{
    return Results.Ok(new
    {
        TotalActiveAlerts = alertService.GetActiveAlertCount(),
        Alerts = alertService.GetActiveAlerts()
    });
});

app.MapGet("/api/alerts/active/{deviceId}/{metric}",
(string deviceId, string metric, ActiveAlertService alertService) =>
{
    return Results.Ok(new
    {
        DeviceId = deviceId,
        Metric = metric,
        IsActive = alertService.ContainsAlert(deviceId, metric)
    });
});

// Simulated device disconnect event.
app.MapPost("/api/alerts/disconnect/{deviceId}",
(string deviceId, ActiveAlertService alertService) =>
{
    bool added = alertService.AddAlert(deviceId, "Disconnected");

    return Results.Ok(new
    {
        DeviceId = deviceId,
        Alert = "Disconnected",
        Added = added
    });
});

// Simulated device reconnection event.
app.MapPost("/api/alerts/reconnect/{deviceId}",
(string deviceId, ActiveAlertService alertService) =>
{
    bool removed = alertService.RemoveAlert(deviceId, "Disconnected");

    return Results.Ok(new
    {
        DeviceId = deviceId,
        Alert = "Disconnected",
        Removed = removed
    });
});





app.MapPost("/api/commands/execute",
(DeviceCommandRequest request, DeviceCommandService commandService, OperatorActivityService activityService) =>
{
    if (string.IsNullOrWhiteSpace(request.DeviceId) ||
    string.IsNullOrWhiteSpace(request.CommandType))
    {
        return Results.BadRequest("Device ID and command type are required.");
    }

    string commandType = request.CommandType.Trim();

    if (!commandType.Equals("Pump", StringComparison.OrdinalIgnoreCase) &&
    !commandType.Equals("Relay", StringComparison.OrdinalIgnoreCase))
    {
        return Results.BadRequest("Command type must be Pump or Relay.");
    }

    var record = commandService.ExecuteCommand(request);


    activityService.RecordCommand(
    record.DeviceId,
    record.CommandType);

    return Results.Ok(record);
});

app.MapPost("/api/commands/undo",
(DeviceCommandService commandService) =>
{
    var undone = commandService.UndoLastCommand();

    return undone == null
    ? Results.NotFound("There are no commands available to undo.")
    : Results.Ok(undone);
});

app.MapGet("/api/commands/history",
(DeviceCommandService commandService) =>
Results.Ok(commandService.GetHistory()));

app.MapGet("/api/commands/states",
(DeviceCommandService commandService) =>
Results.Ok(commandService.GetDeviceStates()));

app.MapGet("/api/commands/undo-count",
(DeviceCommandService commandService) =>
Results.Ok(new
{
    AvailableUndoCommands = commandService.GetUndoCount()
}));


app.MapGet("/api/command-centre/devices",
(DeviceHealthService healthService,
DeviceSimulationService simulationService,
ActiveAlertService alertService,
OperatorActivityService activityService,
string? search,
string? category,
string? alertState) =>
{

    if (!string.IsNullOrWhiteSpace(search))
    {
        string searchedDevice = search.Trim();

        if (DeviceStore.Devices.Any(d =>
        d.DeviceId.Equals(
        searchedDevice,
        StringComparison.OrdinalIgnoreCase)))
        {
            activityService.RecordSearch(searchedDevice);
        }
    }


    simulationService.UpdateSimulatedConnectivity();

    var activeAlerts = alertService.GetActiveAlerts();

    var devices = DeviceStore.Devices.Select(device =>
    {
        var health = healthService.CalculateHealth(device.LastSeen);

        bool hasActiveAlert = activeAlerts.Any(alert =>
        alert.StartsWith(
        device.DeviceId + "|",
        StringComparison.OrdinalIgnoreCase));

        return new
        {
            device.DeviceId,
            device.MacAddress,
            device.DeploymentLocation,
            Category = device.Category.ToString(),
            Health = health.ToString(),
            HasActiveAlert = hasActiveAlert
        };
    }).ToList();

    var filtered = devices.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        filtered = filtered.Where(d =>
        d.DeviceId.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        d.MacAddress.Contains(search, StringComparison.OrdinalIgnoreCase) ||
        d.DeploymentLocation.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    if (!string.IsNullOrWhiteSpace(category) &&
    !category.Equals("All", StringComparison.OrdinalIgnoreCase))
    {
        filtered = filtered.Where(d =>
        d.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    if (!string.IsNullOrWhiteSpace(alertState) &&
    !alertState.Equals("All", StringComparison.OrdinalIgnoreCase))
    {
        filtered = filtered.Where(d =>
        alertState.Equals("Active", StringComparison.OrdinalIgnoreCase)
        ? d.HasActiveAlert
        : alertState.Equals("Disconnected", StringComparison.OrdinalIgnoreCase)
        ? d.Health.Equals("Disconnected", StringComparison.OrdinalIgnoreCase)
        : alertState.Equals("Normal", StringComparison.OrdinalIgnoreCase)
        ? !d.HasActiveAlert &&
        !d.Health.Equals("Disconnected", StringComparison.OrdinalIgnoreCase)
        : true);
    }

    return Results.Ok(filtered.ToList());
});


app.MapGet("/api/operator-activity",
(OperatorActivityService activityService) =>
{
    return Results.Ok(new
    {
        DeviceSearches = activityService.GetSearchCounts(),
        ManualCommands = activityService.GetCommandCounts()
    });
});


app.MapGet("/api/recommendations",
(RecommendationEngineService recommendationService) =>
{
    var recommendations =
    recommendationService.GenerateRecommendations();

    return Results.Ok(new
    {
        TotalRecommendations = recommendations.Count,
        Suggestions = recommendations
    });
});


app.Run();











