Smart-X IoT Mesh Ecosystem

Overview

Smart-X is a simulated IoT management ecosystem developed using .NET 10. The project demonstrates telemetry ingestion, device health monitoring, anomaly detection, deployment hierarchy validation, attachment handling and advanced C# programming concepts for a hybrid IoT environment.

The solution contains:

- SmartX.Api — ASP.NET Core Minimal API backend
- SmartX.Desktop — WPF desktop management interface

The system simulates ESP32 devices that publish environmental, power-consumption and actuator telemetry to a central gateway.



Main Features

### Device Health Monitoring
Registered IoT devices are classified as:

- Healthy
- Warning
- Critical
- Disconnected

Health is calculated using the device's latest telemetry or heartbeat timestamp.

### Live WPF Dashboard
The desktop dashboard displays:

- total registered devices
- device health counts
- device ID and MAC address
- deployment location
- sensor category
- connectivity status
- automatic refresh timestamps
- recent telemetry anomalies

The dashboard automatically refreshes approximately every five seconds.

### Device Registration
New simulated ESP32 devices can be registered from the WPF interface.

Required information includes:

- Device ID
- MAC address
- Deployment location
- Sensor category

Duplicate Device IDs and MAC addresses are rejected.

### Generic Telemetry Processing
The project uses:

'TelemetryPacket<T>'

to process multiple telemetry types without requiring separate packet structures.

Supported data includes:

- 'float' — temperature and soil moisture
- 'int' — power wattage
- 'bool' — actuator or valve states

### Anomaly Detection
Threshold-based anomaly detection identifies abnormal sensor readings such as:

- unusually high or low temperature
- abnormal soil moisture
- excessive power consumption

Detected anomalies are recorded and displayed on the WPF dashboard.

### Simulated ESP32 Connectivity
The system simulates different device connectivity conditions to demonstrate:

- normal transmission
- delayed transmission
- critical communication delays
- disconnected devices

This allows the assessment functionality to be demonstrated without physical ESP32 hardware.

### Advanced C# Concepts

The project demonstrates the following required concepts:

#### Generics
'TelemetryPacket<T>' provides reusable telemetry handling for float, integer and Boolean values.

#### Operator Overloading
'SensorReading' overloads arithmetic and comparison operators to support sensor aggregation and comparison.

#### Jagged Arrays
Historical telemetry batches are initially stored using jagged arrays before being transferred into generic telemetry collections.

#### Recursion
Deployment hierarchy validation recursively traverses facilities, zones and device nodes.

### File and Log Attachments
Users can attach device-related files from the WPF interface using an Open File dialog.

Supported attachments include:

- configuration files
- hardware logs
- deployment photographs
- CSV/XML/JSON files

Files are uploaded to the API using multipart form data.



## Solution Structure

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


Requirements
To run the solution, install:

Visual Studio 2022 or later
.NET 10 SDK
Windows operating system
Git, if cloning from GitHub
Ensure the following Visual Studio workload is installed:

.NET desktop development
ASP.NET and web development

Alternatively run: dotnet restore


3. Configure Startup Projects
In Visual Studio:

Right-click the solution.
Select Configure Startup Projects.
Select Multiple startup projects.
Set the following to Start:
SmartX.Api
SmartX.Desktop
4. Run
Press: F5

or select the green Start button.

The API must be running before the WPF desktop application communicates with it.

The development API uses: https://localhost:7031/


Important API Endpoints:

GET /api/devices
POST /api/devices/register

GET /api/telemetry/float
GET /api/telemetry/int
GET /api/telemetry/bool

POST /api/telemetry/float
POST /api/telemetry/int
POST /api/telemetry/bool

GET /api/telemetry/summary

GET /api/anomalies

GET /api/operator-demo

GET /api/deployment/demo
GET /api/deployment/invalid-demo

POST /api/devices/{deviceId}/attachments
GET /api/devices/{deviceId}/attachments


Testing
API requests can be tested using: SmartX.Api.http


The HTTP request file contains tests for telemetry ingestion, anomaly detection, device attachments and other API functionality.

The WPF interface can also be used to test:

device registration
health monitoring
anomaly display
automatic refresh
file attachment uploads


Mock Data
The assessment does not require physical IoT hardware.

Smart-X therefore generates large volumes of mock telemetry to simulate readings from multiple ESP32 devices.

This demonstrates how the implemented data structures behave under telemetry load.


Data Persistence
This assessment version primarily uses in-memory collections.

This means registered devices, anomaly records and generated telemetry may reset when the API application is restarted.

Uploaded files are stored in the API’s local Uploads directory.

A future version could use a persistent database and cloud storage.


Future Development
The current implementation focuses on Part 1:

Sensor Data Ingestion and Telemetry

The remaining Smart-X pillars are reserved for later stages of the PoE:

Real-Time Command Stream and History
Network Topology and Mesh Routing
Potential future enhancements include database persistence, real ESP32 integration, advanced machine-learning anomaly detection and cloud deployment.


Technologies
C#
.NET 10
ASP.NET Core Minimal API
WPF
XAML
HTTP/REST
Generic Collections
Git and GitHub


REFERENCES


Majib, Y. et al. (2023) ‘Detecting anomalies within smart buildings using do-it-yourself internet of things’, Journal of Ambient Intelligence and Humanized Computing, 14, pp. 4727–4743. doi: 10.1007/s12652-022-04376-w.

Microsoft (2026) Generics in .NET. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/standard/generics/ (Accessed: 13 September 2026).

Mishra, A., Cohen, A., Reichherzer, T. and Wilde, N. (2021) ‘Detection of data anomalies at the edge of pervasive IoT systems’, Computing, 103, pp. 1657–1675. doi: 10.1007/s00607-021-00927-9.

Microsoft (2026) Arrays – C# reference. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/arrays (Accessed: 13 September 2026).

Microsoft (2026) Operator overloading – C# reference. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading (Accessed: 13 September 2026).







