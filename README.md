Smart-X IoT Mesh Ecosystem
Demo Part 1: https://www.youtube.com/watch?v=8jRu0zl8MOw

Overview

Smart-X is a simulated Internet of Things (IoT) management ecosystem developed using C# and .NET 10.

The project demonstrates telemetry ingestion, device health monitoring, anomaly detection, deployment hierarchy validation, file attachments, real-time command processing, advanced data structures and predictive recommendations within a simulated IoT environment.

The solution implements two main pillars:

- Part 1: Sensor Data Ingestion and Telemetry
- Part 2: Real-Time Command Stream and History

The third pillar, Network Topology and Mesh Routing, is reserved for the final Portfolio of Evidence (PoE).

The solution consists of two applications:

- SmartX.Api — ASP.NET Core Minimal API backend
- SmartX.Desktop — WPF desktop management interface

The system simulates ESP32 devices that transmit environmental, power-consumption and actuator telemetry to a central API gateway.

Physical ESP32 hardware is not required.

---

Part 1 — Sensor Data Ingestion and Telemetry

1. Device Health Monitoring

Registered IoT devices are classified into four health states:

- Healthy
- Warning
- Critical
- Disconnected

Device health is calculated using the latest telemetry or heartbeat timestamp.

The system also simulates connectivity changes to demonstrate how delayed or missing telemetry affects device health.

2. Live WPF Dashboard

The Part 1 desktop dashboard displays:

- Total registered devices
- Device health counts
- Device IDs and MAC addresses
- Deployment locations
- Sensor categories
- Connectivity status
- Device health messages
- Recent telemetry anomalies
- Automatic refresh information

The dashboard refreshes approximately every five seconds.

3. Device Registration

New simulated ESP32 devices can be registered through the WPF interface.

Required information includes:

- Device ID
- MAC address
- Deployment location
- Sensor category

Duplicate Device IDs and MAC addresses are rejected.

Successfully registered devices are added to the in-memory device collection.

4. Generic Telemetry Processing

The project uses the generic class:

`TelemetryPacket<T>`

This allows different telemetry types to be processed using reusable code.

Supported data types include:

- `float` — temperature and soil moisture
- `int` — power consumption in watts
- `bool` — actuator or valve states

5. Anomaly Detection

Threshold-based anomaly detection identifies abnormal sensor readings, including:

- Unusually high or low temperatures
- Abnormal soil moisture
- Excessive power consumption

Detected anomalies are recorded and displayed on the WPF dashboard.

6. Simulated ESP32 Connectivity

The system simulates different connectivity conditions:

- Normal transmission
- Delayed transmission
- Critical communication delays
- Disconnected devices

This demonstrates device health monitoring without requiring physical IoT hardware.

7. Advanced C# Concepts


####Generics

`TelemetryPacket<T>` provides reusable telemetry processing for float, integer and Boolean values.

#### Operator Overloading

`SensorReading` overloads arithmetic and comparison operators to support sensor aggregation and comparison.

#### Jagged Arrays

Historical telemetry batches are initially stored using jagged arrays before being transferred into generic telemetry collections.

#### Recursion

Deployment hierarchy validation recursively traverses facilities, zones and device nodes.

8. File and Log Attachments

Users can attach device-related files through the WPF interface using an Open File dialog.

Supported attachments include:

- Configuration files
- Hardware logs
- Deployment photographs
- CSV files
- XML files
- JSON files

Files are uploaded to the API using multipart form data.

---

Part 2 — Real-Time Command Stream and History

1. Real-Time Command Centre

Part 2 introduces a WPF Command Centre that allows operators to monitor incoming telemetry and interact with simulated IoT devices.

The interface provides:

- Live telemetry monitoring
- Queued telemetry counters
- Critical alert counters
- Active device information
- Command execution counters
- Device searching and filtering
- Manual Pump and Relay controls
- Command history
- Undo functionality
- Predictive suggested actions

The Command Centre automatically refreshes approximately every three seconds.

2. Advanced Collections

Part 2 demonstrates the use of advanced C# collection types to manage real-time telemetry, commands and device information.

#### Queue<T> — FIFO Telemetry Processing

Normal incoming telemetry is stored using:

`Queue<QueuedTelemetry>`

Messages are processed in First-In, First-Out (FIFO) order.

This ensures normal telemetry messages are processed in their arrival order.

#### PriorityQueue<TElement, TPriority> — Critical Telemetry

Critical telemetry is managed using:

`PriorityQueue<QueuedTelemetry, int>`

Critical messages receive higher processing priority than normal queued messages.

Examples of critical telemetry include:

- Temperature below 10°C or above 40°C
- Soil moisture below 20% or above 90%
- Power consumption below 0 W or above 5000 W

The queue processor checks critical telemetry before normal telemetry.

A dedicated priority test endpoint demonstrates this behaviour.

#### Stack<T> — Command Undo

A stack stores reversible device commands.

The system uses:

`Stack<DeviceCommandRecord>`

Operators can execute simulated Pump and Relay commands and undo the most recent reversible command.

The Last-In, First-Out (LIFO) behaviour ensures the most recent command is undone first.

#### Dictionary<TKey, TValue> — Device Lookup

Dictionaries support efficient device lookup using unique identifiers.

The project includes:

- Lookup of devices and their latest telemetry
- Lookup of registered devices using Device IDs
- Synchronisation of the registered-device Dictionary after successful registration

Dictionary lookup provides average-case constant-time access to device information.

#### SortedDictionary<TKey, TValue> — Telemetry History

Historical telemetry is maintained using:

`SortedDictionary<DateTime, List<QueuedTelemetry>>`

This allows telemetry records to be organised by timestamp in chronological order.

The history can be retrieved through the API.

#### HashSet<T> — Unique Active Alerts

The project uses:

`HashSet<string>`

to track unique active alerts.

Each alert is identified using a combination of Device ID and telemetry metric.

This prevents duplicate entries for the same active alert.

The system also supports simulated device disconnection and reconnection alerts.

3. HTTP-Based ESP32 Simulator

Part 2 includes a background service that simulates telemetry transmission from ESP32 devices.

The simulator:

- Generates readings from four simulated ESP32 devices
- Produces environmental and power-related telemetry
- Sends telemetry to the API using HTTP POST requests
- Occasionally generates critical readings
- Operates automatically while the API is running

The simulator uses `IHttpClientFactory` and sends telemetry to:

`POST /api/queue/enqueue`

This ensures that simulated telemetry enters the system through the API rather than being inserted directly into the queue.

4. Background Telemetry Processing

A background processing service automatically processes queued telemetry.

The processor:

- Checks for incoming telemetry
- Processes critical messages before normal messages
- Maintains a collection of processed telemetry
- Operates approximately once per second

This allows telemetry processing to continue without requiring operators to manually trigger each message.

5. Device Search and Filtering

The Command Centre supports device discovery and filtering.

Operators can:

- Search for devices using their identifiers
- Filter devices by category
- Filter devices by alert state
- View matching registered devices

This makes it easier to locate and monitor devices in the simulated ecosystem.

6. Manual Device Commands

Operators can execute simulated commands through the WPF interface.

Supported commands include:

- Pump ON
- Pump OFF
- Relay ON
- Relay OFF

The system records command activity and maintains simulated device states.

A command history allows operators to review previously executed actions.

7. Command Undo

The Undo feature uses a stack to reverse the most recent reversible command.

The system maintains:

- Command history
- Simulated device states
- Undo stack
- Available undo count

This demonstrates practical use of stack-based LIFO processing.

### 8. Predictive Recommendation Engine

The system includes a rule-based predictive recommendation engine.

It analyses historical telemetry and operator behaviour to suggest possible actions.

Recommendation conditions include:

- Repeated critical temperature readings
- Elevated average temperatures
- Existing active alerts
- Repeated searches for the same device
- Frequent manual command execution

Suggested actions are displayed in the WPF Command Centre.

The recommendation engine uses defined rules and historical activity rather than a trained machine-learning model.

---

Solution Structure

```text
SmartX
│
├── SmartX.Api
│ ├── Data
│ ├── Models
│ ├── Services
│ ├── Uploads
│ ├── Program.cs
│ └── SmartX.Api.http
│
└── SmartX.Desktop
├── Models
├── Services
├── Views
├── MainWindow.xaml
└── MainWindow.xaml.cs
```

---

Technologies Used

- C#
- .NET 10
- ASP.NET Core Minimal API
- Windows Presentation Foundation (WPF)
- XAML
- HTTP/REST
- Generic collections
- Queue and PriorityQueue
- Stack
- Dictionary and SortedDictionary
- HashSet
- BackgroundService
- IHttpClientFactory
- Git and GitHub
- Visual Studio

---

System Requirements

To run the solution, install:

- Visual Studio 2022 or later with support for .NET 10
- .NET 10 SDK
- Windows operating system
- Git, if cloning from GitHub

Required Visual Studio workloads:

- .NET desktop development
- ASP.NET and web development

---

Installation and Setup

Step 1 — Open the Solution

Clone or download the GitHub repository.

Open the Smart-X solution in a compatible version of Visual Studio.

Step 2 — Restore Dependencies

Restore NuGet packages using Visual Studio or run:

```bash
dotnet restore
```

Step 3 — Configure Startup Projects

In Visual Studio:

1. Right-click the solution.
2. Select **Configure Startup Projects**.
3. Select **Multiple startup projects**.
4. Set the following projects to **Start**:
- SmartX.Api
- SmartX.Desktop
5. Apply the changes.

Step 4 — Run the Solution

Press `F5` or select the Start button.

The API must be running before the WPF desktop application can communicate with it.

The development API uses:

`https://localhost:7031/`

If HTTPS certificate trust is required, run:

```bash
dotnet dev-certs https --trust
```

### Step 5 — Navigate the Application

The main menu provides access to:

- Part 1: Sensor Data Ingestion and Telemetry
- Part 2: Real-Time Command Stream and History

The Part 3 navigation option is reserved for the final PoE.

---

## Important API Endpoints

### Part 1 — Device Management

```http
GET /api/devices
POST /api/devices/register
```

### Part 1 — Telemetry

```http
GET /api/telemetry/float
GET /api/telemetry/int
GET /api/telemetry/bool

POST /api/telemetry/float
POST /api/telemetry/int
POST /api/telemetry/bool

GET /api/telemetry/summary
```

### Part 1 — Anomalies and Demonstrations

```http
GET /api/anomalies
GET /api/operator-demo

GET /api/deployment/demo
GET /api/deployment/invalid-demo
```

### Part 1 — Device Attachments

```http
POST /api/devices/{deviceId}/attachments
GET /api/devices/{deviceId}/attachments
```

### Part 2 — Telemetry Queue

```http
POST /api/queue/enqueue
POST /api/queue/process-next

GET /api/queue/status
GET /api/queue/processed
GET /api/queue/priority-test
GET /api/queue/history
```

### Part 2 — Device Lookup

```http
GET /api/queue/devices
GET /api/queue/devices/{deviceId}

GET /api/registered-devices/count
GET /api/registered-devices/{deviceId}

GET /api/command-centre/devices
```

The Command Centre device endpoint supports search and filtering parameters.

### Part 2 — Active Alerts

```http
GET /api/alerts/active
GET /api/alerts/active/{deviceId}/{metric}

POST /api/alerts/disconnect/{deviceId}
POST /api/alerts/reconnect/{deviceId}
```

### Part 2 — Device Commands

```http
POST /api/commands/execute
POST /api/commands/undo

GET /api/commands/history
GET /api/commands/states
GET /api/commands/undo-count
```

### Part 2 — Recommendations

```http
POST /api/operator-activity
GET /api/recommendations
```

---

Testing

The project can be tested using:

- Visual Studio
- SmartX.Api.http
- WPF desktop interface
- API responses
- Background ESP32 simulator

### Part 1 Testing

Use the WPF interface and API endpoints to test:

1. Device registration
2. Duplicate Device ID and MAC address validation
3. Telemetry ingestion
4. Health monitoring
5. Anomaly detection
6. Automatic dashboard refresh
7. File attachment uploads
8. Deployment hierarchy validation

### Part 2 — Priority Queue Test

A dedicated endpoint demonstrates priority-based processing.

**Request:**

```http
GET https://localhost:7031/api/queue/priority-test
```

The test creates three isolated telemetry messages:

- TEST-001 — Normal
- TEST-002 — Normal
- TEST-003 — Critical

**Expected processing order:**

```text
1. TEST-003 — Critical
2. TEST-001 — Normal
3. TEST-002 — Normal
```

This confirms that critical telemetry is processed before normal messages while normal messages retain FIFO ordering.

### Part 2 — Dictionary Lookup Test

**Step 1: Check registered device count**

```http
GET https://localhost:7031/api/registered-devices/count
```

Confirm that the API returns the number of registered devices.

**Step 2: Search for an existing device**

```http
GET https://localhost:7031/api/registered-devices/ESP32-0001
```

Confirm that the registered device is returned.

**Step 3: Search for an unknown device**

```http
GET https://localhost:7031/api/registered-devices/UNKNOWN-999
```

Confirm that the API handles the missing device appropriately.

**Step 4: Register a new device**

Use the existing device registration endpoint or the WPF registration interface to register a device with a unique Device ID and MAC address.

Repeat the count request.

Confirm that:

- The registered device count increases
- The new device can be retrieved using its Device ID
- The Dictionary remains synchronised with device registration

### Part 2 — Sorted Telemetry History Test

**Request:**

```http
GET https://localhost:7031/api/queue/history
```

Inspect the returned historical telemetry records.

Confirm that telemetry history is maintained using timestamp-based ordering.

### Part 2 — HashSet Alert Test

**Request:**

```http
GET https://localhost:7031/api/alerts/active
```

Confirm that active alerts are returned.

Generate critical telemetry or use the simulated disconnection and reconnection endpoints.

Verify that duplicate active alert entries are prevented.

### Part 2 — Manual Command and Undo Test

1. Run the API and WPF desktop application.
2. Open the Real-Time Command Centre.
3. Select a simulated device.
4. Execute a Pump or Relay command.
5. Confirm that the command is recorded.
6. Check the simulated device state.
7. Select Undo.
8. Confirm that the previous simulated state is restored.

This demonstrates stack-based command reversal.

### Part 2 — Live Telemetry Test

1. Start SmartX.Api.
2. Start SmartX.Desktop.
3. Open the Command Centre.
4. Observe incoming telemetry from the HTTP ESP32 simulator.
5. Confirm that dashboard information updates automatically.
6. Monitor queued telemetry and critical alert counters.
7. Confirm that processed telemetry is displayed.

The ESP32 simulator sends telemetry through HTTP to the API.

### Part 2 — Predictive Recommendations Test

1. Open the Command Centre.
2. Observe the Suggested Actions section.
3. Generate repeated critical telemetry readings.
4. Perform repeated device searches or manual commands.
5. Allow the dashboard to refresh.
6. Confirm that recommendations appear when the configured conditions are satisfied.

Recommendations are generated using telemetry history, active alerts and operator activity.

---

Mock Data and Simulation

The assessment does not require physical ESP32 hardware.

Smart-X generates simulated telemetry to demonstrate how IoT devices communicate with the system.

Part 1 provides mock sensor readings and connectivity behaviour.

Part 2 extends this functionality using an HTTP-based ESP32 simulator that automatically transmits telemetry to the API.

The simulator allows testing of:

- Normal telemetry processing
- Critical telemetry prioritisation
- Real-time dashboard updates
- Alert generation
- Historical telemetry
- Predictive recommendations

---

Data Persistence

This assessment primarily uses in-memory collections.

The application maintains runtime information using collections such as:

- List
- Queue
- PriorityQueue
- Stack
- Dictionary
- SortedDictionary
- HashSet

Registered devices, telemetry records, command history, active alerts and recommendation-related activity may reset when the API restarts.

Uploaded files are stored in the API's local Uploads directory.

A future implementation could introduce persistent database storage and cloud integration.

---

Future Development

Parts 1 and 2 of the Smart-X IoT Mesh Ecosystem have been implemented.

The remaining pillar is:

Part 3 — Network Topology and Mesh Routing**

This is reserved for the final PoE.

Potential future enhancements include:

- Persistent database storage
- Real ESP32 hardware integration
- Cloud deployment
- Advanced machine-learning-based predictions
- Network topology visualisation
- Mesh routing and network optimisation
- Expanded telemetry analytics

---

Version Control

The project is managed using Git and GitHub.

Development changes are committed incrementally to document the implementation of:

- Part 1 telemetry functionality
- Device health monitoring
- API development
- WPF interface development
- Advanced collections
- HTTP telemetry simulation
- Device command processing
- Predictive recommendations
- Testing and documentation

The repository commit history provides evidence of progressive development.


REFERENCES

Majib, Y. et al. (2023) ‘Detecting anomalies within smart buildings using do-it-yourself internet of things’, Journal of Ambient Intelligence and Humanized Computing, 14, pp. 4727–4743. doi: 10.1007/s12652-022-04376-w.

Microsoft (2026) Generics in .NET. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/standard/generics/ (Accessed: 13 September 2026).

Mishra, A., Cohen, A., Reichherzer, T. and Wilde, N. (2021) ‘Detection of data anomalies at the edge of pervasive IoT systems’, Computing, 103, pp. 1657–1675. doi: 10.1007/s00607-021-00927-9.

Microsoft (2026) Arrays – C# reference. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/arrays (Accessed: 13 September 2026).

Microsoft (2026) Operator overloading – C# reference. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading (Accessed: 13 September 2026).

Microsoft (2026) PriorityQueue<TElement, TPriority> class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2 (Accessed: 19 October 2026).

Microsoft (2026) Dictionary<TKey, TValue> class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 (Accessed: 19 October 2026).

Microsoft (2026) SortedDictionary<TKey, TValue> class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2 (Accessed: 19 October 2026).

Microsoft (2026) HashSet<T> class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 (Accessed: 19 October 2026).

Microsoft (2026) BackgroundService class. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.backgroundservice (Accessed: 19 October 2026).





