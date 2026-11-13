Smart-X IoT Mesh Ecosystem

POE DEMO : https://youtu.be/9Npb_dsEv6o?si=pH63MQlHzIFb4bvG

Demo Part 1: https://www.youtube.com/watch?v=8jRu0zl8MOw
**Demo Part 2:** https://youtu.be/KevP-UWkXMo?si=3SVRvszNwb95SHmL 

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



## Smart-X IoT Mesh Ecosystem

Part 3 — Network Topology, Mesh Routing and Historical Telemetry

1. POE  overview

Part 3 extends Smart-X with a network-topology dashboard and historical telemetry retrieval. The backend models a simulated ESP32 mesh network, calculates reachability and weighted routes, and exposes these operations through HTTP endpoints. The WPF client displays the nodes, weighted links, algorithm results, fault conditions and recovery behaviour. Historical timestamped readings can be retrieved by device and date range using an AVL-tree-backed service.

The principal implemented features are:

● A weighted, undirected graph of one gateway and four ESP32 nodes, represented using an adjacency list.

● Breadth-First Search (BFS) to traverse reachable devices and report hop counts and unreachable nodes.

● Prim’s Minimum Spanning Tree (MST) to select a minimum-total-weight set of links connecting reachable nodes.

● Dijkstra’s shortest-path algorithm to calculate the least-cost route from the gateway to a selected destination (the current UI demonstrates ESP32-0004).

● Fault simulation to disconnect/reconnect devices, redraw online/offline connections, recalculate a route and report degraded or restored network status.

● AVL-tree-based historical telemetry range queries by device and timestamp, presented in a WPF data grid.

How this fits the full application

The main menu connects the three project areas: Sensor Data Ingestion & Telemetry (Part 1), Real-Time Command Stream & History (Part 2), and Network Topology & Mesh Routing (Part 3). The API is responsible for data structures and algorithms; the WPF application requests results and renders them.

2. Architecture and data flow

SmartX.Desktop (WPF)

  ├─ Main dashboard

  └─ MeshTopologyWindow

       ├─ Graph visualisation (Canvas)

       ├─ BFS / Prim / Dijkstra controls

       ├─ Device disconnect / reconnect controls

       ├─ Network recovery status

       └─ Historical telemetry date-range search

                  │ HTTP / JSON

                  ▼

SmartX.Api (ASP.NET Core)

  ├─ MeshGraphService

  │    ├─ Node dictionary and weighted adjacency lists

  │    ├─ BFS traversal

  │    ├─ Prim MST

  │    ├─ Dijkstra shortest path

  │    └─ Device online/offline state

  └─ AVL historical telemetry service

       └─ Timestamp-keyed, balanced search tree / range query

Separation of concerns: The API owns the network state and calculates results; the desktop client consumes those results. The graph’s node positions in the WPF Canvas are fixed for the five seeded demonstration nodes, but node status, connections, weights and routes come from API responses.

3. Data structure explanations 

3.1 AVL Tree — historical timestamped telemetry

What an AVL tree is

An AVL tree is a self-balancing binary search tree (BST). Each node stores a key and associated value or values. For a timestamped telemetry index, the key is the reading’s timestamp; earlier timestamps are stored in the left subtree and later timestamps in the right subtree. The AVL invariant keeps the height difference between a node’s left and right subtrees within −1, 0 or +1.

A normal BST can become severely unbalanced when readings arrive in chronological order: successive timestamps can create a long chain. The AVL tree counteracts this by performing left, right, left-right or right-left rotations after insertion when required. Its height therefore remains O(log n).

Role in Smart-X

Part 3 requires users to retrieve a particular sensor’s readings within a start and end time, such as moisture measurements during a specified period. The desktop Historical Sensor Telemetry section allows users to choose an ESP32 device and two dates, then calls:

GET /api/avl/history?deviceId=ESP32-0001&start=2026-01-01T00:00:00&end=2026-12-31T23:59:59

The API returns matching readings; the desktop displays ReceivedAt, DeviceId, Metric, Value and Unit. The query should prune irrelevant subtrees based on timestamp bounds, rather than scanning every stored reading.

Why this improves efficiency

|Operation                        |Unbalanced BST (worst case)|AVL tree    |
|---------------------------------|--------------------------:|-----------:|
|Insert one timestamp             |O(n)                       |O(log n)    |
|Find one timestamp               |O(n)                       |O(log n)    |
|Range query returning `k` matches|O(n)                       |O(log n + k)|
|Extra storage                    |O(n)                       |O(n)        |

Here, n is the number of indexed timestamps and k is the number of returned readings. The range-query bound assumes the tree is correctly balanced and the query prunes subtrees outside the interval. If readings share a timestamp, the index must accommodate duplicates, for example by storing multiple readings per timestamp or by using a composite key.

Relevant C# example: AVL insertion and balancing

The following is a simplified illustrative implementation of the AVL balancing mechanism. It demonstrates the core algorithm; it is not intended to replace the repository’s AVL service.

public sealed class AvlNode

{

    public DateTime Timestamp { get; set; }

    public int Height { get; set; } = 1;

    public AvlNode? Left { get; set; }

    public AvlNode? Right { get; set; }



    public AvlNode(DateTime timestamp) => Timestamp = timestamp;

}



private static int Height(AvlNode? node) => node?.Height ?? 0;

private static int Balance(AvlNode node) =>

    Height(node.Left) - Height(node.Right);



private static void RefreshHeight(AvlNode node) =>

    node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));



private static AvlNode RotateRight(AvlNode root)

{

    AvlNode pivot = root.Left!;

    root.Left = pivot.Right;

    pivot.Right = root;

    RefreshHeight(root);

    RefreshHeight(pivot);

    return pivot;

}



private static AvlNode RotateLeft(AvlNode root)

{

    AvlNode pivot = root.Right!;

    root.Right = pivot.Left;

    pivot.Left = root;

    RefreshHeight(root);

    RefreshHeight(pivot);

    return pivot;

}



private static AvlNode Insert(AvlNode? root, DateTime timestamp)

{

    if (root is null) return new AvlNode(timestamp);



    if (timestamp < root.Timestamp)

        root.Left = Insert(root.Left, timestamp);

    else if (timestamp > root.Timestamp)

        root.Right = Insert(root.Right, timestamp);

    else

        return root; // Illustration only: handle duplicate readings in production.



    RefreshHeight(root);

    int balance = Balance(root);



    if (balance > 1 && timestamp < root.Left!.Timestamp)

        return RotateRight(root); // Left-left



    if (balance < -1 && timestamp > root.Right!.Timestamp)

        return RotateLeft(root); // Right-right



    if (balance > 1 && timestamp > root.Left!.Timestamp)

    {

        root.Left = RotateLeft(root.Left!);

        return RotateRight(root); // Left-right

    }



    if (balance < -1 && timestamp < root.Right!.Timestamp)

    {

        root.Right = RotateRight(root.Right!);

        return RotateLeft(root); // Right-left

    }



    return root;

}

Explanation: Height measures subtree depth. Balance detects whether a node has become too left-heavy or right-heavy. Rotations restore the AVL height constraint while preserving the BST timestamp ordering. This is especially relevant for continuously arriving IoT readings, which often have increasing timestamps.

Relevant C# example: pruned date-range query

private static void SearchRange(

    AvlNode? node,

    DateTime start,

    DateTime end,

    List<DateTime> matches)

{

    if (node is null) return;



    if (start < node.Timestamp)

        SearchRange(node.Left, start, end, matches);



    if (node.Timestamp >= start && node.Timestamp <= end)

        matches.Add(node.Timestamp);



    if (node.Timestamp < end)

        SearchRange(node.Right, start, end, matches);

}

Explanation: If a node’s timestamp is earlier than the requested start, its entire left subtree is also too early and can be skipped. Similarly, when a node is later than the end, its right subtree can be skipped. In the real telemetry index, the matching timestamp corresponds to one or more full sensor readings, not merely a DateTime value.

Integration into the Network Topology feature

The AVL tree complements the graph: the graph answers where a device is connected and how data could travel, while the AVL tree answers what a device measured during a particular time interval. Selecting the same ESP32 identifier in both areas connects live network monitoring to historical sensor analysis. The tree is a storage/query structure; a graphical tree diagram is not required for it to operate correctly, although a visualisation can provide extra evidence of balancing.

3.2 Weighted Graph — ESP32 network topology

What a graph is

A graph consists of vertices (nodes) and edges (connections). Smart-X models each ESP32 sensor and the central gateway as vertices. An edge represents a possible wireless link between two devices. The links are undirected in the demonstration graph, so the connection is traversable in either direction when both endpoints are online. Each edge has a weight representing a simulated relative link cost.

The graph is represented with dictionaries and adjacency lists rather than a dense adjacency matrix. This is suitable for a mesh in which each device connects to a limited subset of other devices.

Role in Smart-X

The MeshGraphService maintains a node lookup and neighbouring weighted links. Its seeded network contains:

Nodes: GATEWAY-001, ESP32-0001, ESP32-0002, ESP32-0003, ESP32-0004.

|Connection              |Weight|
|------------------------|-----:|
|GATEWAY-001 — ESP32-0001|2     |
|GATEWAY-001 — ESP32-0002|4     |
|ESP32-0001 — ESP32-0002 |1     |
|ESP32-0001 — ESP32-0003 |5     |
|ESP32-0002 — ESP32-0003 |2     |
|ESP32-0002 — ESP32-0004 |6     |
|ESP32-0003 — ESP32-0004 |3     |

These are demonstration weights, not measured battery consumption or real radio transmission costs. A future deployment could derive weights from RSSI, packet loss, latency or energy usage.

Relevant C# example: adjacency-list representation

private readonly Dictionary<string, MeshNode> _nodes =

    new(StringComparer.OrdinalIgnoreCase);



private readonly Dictionary<string, List<MeshEdge>> _adjacency =

    new(StringComparer.OrdinalIgnoreCase);



// Illustrative helper for an undirected graph:

private void AddConnection(string from, string to, double weight)

{

    _adjacency[from].Add(new MeshEdge

    {

        SourceId = from,

        DestinationId = to,

        Weight = weight

    });



    _adjacency[to].Add(new MeshEdge

    {

        SourceId = to,

        DestinationId = from,

        Weight = weight

    });

}

Explanation: _nodes provides direct lookup by device identifier. _adjacency[nodeId] contains only that device’s neighbours, allowing algorithms to explore local connections efficiently. An undirected connection is stored in both directions for traversal; the topology API can still return each physical link once for display. The exact MeshEdge model and construction syntax should be checked against MeshGraphService.cs.

Why this improves efficiency

Let V be the number of devices and E the number of links. An adjacency-list graph uses O(V + E) storage, compared with O(V²) for an adjacency matrix. Looking up a device by dictionary key is O(1) on average; iterating its neighbours is O(degree of that device). This matters as the network grows because most mesh devices are not directly connected to every other device.

BFS traversal: connected and unreachable devices

Breadth-First Search uses a queue to visit the gateway first, then its immediate neighbours, then their neighbours. It is appropriate for identifying which nodes are reachable and calculating the minimum number of hops in an unweighted sense.

var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

var queue = new Queue<string>();

var hopCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);



visited.Add("GATEWAY-001");

queue.Enqueue("GATEWAY-001");

hopCounts["GATEWAY-001"] = 0;



while (queue.Count > 0)

{

    string current = queue.Dequeue();



    foreach (var edge in _adjacency[current])

    {

        string neighbour = edge.DestinationId;

        if (!_nodes[neighbour].IsOnline || !visited.Add(neighbour))

            continue;



        hopCounts[neighbour] = hopCounts[current] + 1;

        queue.Enqueue(neighbour);

    }

}

Explanation: The queue ensures level-by-level traversal; the visited set prevents repeated processing in a cyclic mesh. Offline neighbours are skipped. BFS runs in O(V + E) time and uses O(V) additional memory. BFS hop counts do not account for edge weights, so BFS is not interchangeable with Dijkstra for weighted least-cost routing.

WPF graph visualisation and fault simulation

MeshTopologyWindow retrieves GET /api/mesh/topology and draws circles for nodes and lines for links on a WPF Canvas. Blue indicates the gateway, green an online ESP32 device, and red an offline device. Links touching an offline device are grey and dashed. The currently calculated route is highlighted in orange.

Disconnect/reconnect buttons call the API to change a node’s status. After a change, the desktop refreshes the graph, recalculates the route and updates the network recovery message. The network can remain operational through an alternative route even when one sensor is offline.

3.3 Minimum Spanning Tree — Prim’s algorithm

What an MST is

A minimum spanning tree of a connected, weighted, undirected graph is a subset of edges that:

1. Connects every vertex in the graph.

2. Contains no cycles.

3. Has the smallest possible sum of edge weights.

For V vertices, a spanning tree has exactly V − 1 edges. If the available online network is disconnected, an MST cannot span every original node; the algorithm can instead report a tree for the reachable component and identify unreachable nodes.

Role in Smart-X

Smart-X uses Prim’s algorithm to find a low-total-weight connection backbone starting from GATEWAY-001. This helps explain how a mesh could minimise the combined cost of a set of active links while retaining connectivity. In the seeded, fully online network, Prim’s MST selects four edges with a total weight of 8:

|Selected MST connection |Weight|
|------------------------|-----:|
|ESP32-0001 — ESP32-0002 |1     |
|GATEWAY-001 — ESP32-0001|2     |
|ESP32-0002 — ESP32-0003 |2     |
|ESP32-0003 — ESP32-0004 |3     |
|**Total**               |**8** |

The exact order of these selected links can vary depending on tie-breaking and traversal, but the total minimum weight remains 8 for the fully online seeded graph.

Relevant C# example: Prim’s algorithm

var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

var selected = new List<MeshEdge>();

var frontier = new PriorityQueue<MeshEdge, double>();



void AddFrontier(string nodeId)

{

    visited.Add(nodeId);



    foreach (var edge in _adjacency[nodeId])

    {

        if (_nodes[edge.DestinationId].IsOnline &&

            !visited.Contains(edge.DestinationId))

        {

            frontier.Enqueue(edge, edge.Weight);

        }

    }

}



AddFrontier("GATEWAY-001");



while (frontier.Count > 0)

{

    MeshEdge edge = frontier.Dequeue();



    if (visited.Contains(edge.DestinationId))

        continue; // Would introduce a cycle.



    selected.Add(edge);

    AddFrontier(edge.DestinationId);

}



double totalWeight = selected.Sum(edge => edge.Weight);

Explanation: The priority queue always exposes the lowest-weight candidate connection. visited prevents the algorithm from adding an edge that would form a cycle with already selected nodes. Every time a new node joins the tree, its outgoing edges are added to the frontier. With a binary-heap priority queue and adjacency lists, a standard lazy implementation runs in O(E log E) time (often described as O(E log V) for simple graphs) and uses O(V + E) space.

Contribution to efficiency — and an important limitation

The MST minimises the total weight of the selected network backbone. It does not guarantee that the path between any two particular devices is the shortest path in the original graph. It is therefore appropriate for analysing an economical network-wide connection structure, whereas Dijkstra’s algorithm is used to calculate the lowest-cost route for an individual packet or destination.

Furthermore, a lower simulated link weight is not automatically proof of lower real-world power consumption. The demonstration models relative routing cost; physical energy efficiency would require measured or justified energy-based weights.

3.4 Dijkstra’s algorithm — shortest weighted route

Dijkstra’s algorithm complements the MST by finding the minimum-total-weight path from the gateway to a specific ESP32 destination, assuming non-negative edge weights. It maintains the lowest known cost to each reachable node and repeatedly explores the node with the smallest tentative cost.

var costs = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)

{

    ["GATEWAY-001"] = 0

};



var previous = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

var queue = new PriorityQueue<string, double>();

queue.Enqueue("GATEWAY-001", 0);



while (queue.TryDequeue(out string? current, out double currentCost))

{

    if (currentCost > costs[current]) continue;



    foreach (var edge in _adjacency[current])

    {

        if (!_nodes[edge.DestinationId].IsOnline) continue;



        double candidate = currentCost + edge.Weight;

        if (!costs.TryGetValue(edge.DestinationId, out double best) ||

            candidate < best)

        {

            costs[edge.DestinationId] = candidate;

            previous[edge.DestinationId] = current;

            queue.Enqueue(edge.DestinationId, candidate);

        }

    }

}

The previous dictionary allows the selected path to be reconstructed from the destination back to the gateway. A priority-queue implementation typically runs in O((V + E) log V) time for a simple graph.

Demonstrated behaviour:

|Network state         |Shortest route to ESP32-0004                                   |Cost|Hops|
|----------------------|---------------------------------------------------------------|---:|---:|
|All nodes online      |GATEWAY-001 → ESP32-0001 → ESP32-0002 → ESP32-0003 → ESP32-0004|8   |4   |
|ESP32-0002 offline    |GATEWAY-001 → ESP32-0001 → ESP32-0003 → ESP32-0004             |10  |3   |
|ESP32-0002 reconnected|Original optimal route restored                                |8   |4   |

The failover path has fewer hops but higher cost, showing why weighted routing is more informative than hop count alone. The WPF graph highlights the current path in orange and marks unavailable links as dashed.

3.5 Comparison of the Part 3 data structures and algorithms

|Component                    |Primary purpose                               |Typical complexity                        |Benefit to Smart-X                           |
|-----------------------------|----------------------------------------------|------------------------------------------|---------------------------------------------|
|AVL tree                     |Index timestamped telemetry                   |Insert/search O(log n); range O(log n + k)|Fast historical queries over growing data    |
|Weighted adjacency-list graph|Store device connectivity                     |Space O(V + E)                            |Scalable, sparse network representation      |
|BFS                          |Find reachable nodes and minimum hop counts   |O(V + E)                                  |Detect disconnected/unreachable devices      |
|Prim MST                     |Minimise total weight of a connecting backbone|O(E log E) with heap                      |Analyse economical network-wide links        |
|Dijkstra                     |Find minimum-weight path to a destination     |O((V + E) log V)                          |Recalculate an alternative route after faults|

These structures are complementary rather than competing. The AVL tree indexes historical measurements; the graph models current connectivity; BFS assesses reachability; Prim selects a minimum-weight backbone; and Dijkstra calculates an individual least-cost route.

4. Part 3 API endpoints

The development API used by the WPF client is http://localhost:5016/.

|Method|Endpoint                                                                                |Purpose                             |
|------|----------------------------------------------------------------------------------------|------------------------------------|
|GET   |`/api/mesh/topology`                                                                    |Return nodes, links and totals      |
|GET   |`/api/mesh/bfs/GATEWAY-001`                                                             |Traverse the reachable network      |
|GET   |`/api/mesh/mst/GATEWAY-001`                                                             |Calculate Prim’s MST                |
|GET   |`/api/mesh/shortest-path/GATEWAY-001/ESP32-0004`                                        |Calculate the weighted shortest path|
|PUT   |`/api/mesh/nodes/ESP32-0002/status?isOnline=false`                                      |Simulate a device disconnect        |
|PUT   |`/api/mesh/nodes/ESP32-0002/status?isOnline=true`                                       |Restore a device connection         |
|GET   |`/api/avl/history?deviceId=ESP32-0001&start=2026-01-01T00:00:00&end=2026-12-31T23:59:59`|Query historical telemetry          |

Note: Endpoint examples reflect the currently implemented demonstration. Device IDs, available historical records and the port depend on the running application state and configuration.

Example: retrieve topology

Invoke-RestMethod http://localhost:5016/api/mesh/topology

Example: disconnect and reconnect a device

Invoke-RestMethod -Method Put `

  "http://localhost:5016/api/mesh/nodes/ESP32-0002/status?isOnline=false"



Invoke-RestMethod -Method Put `

  "http://localhost:5016/api/mesh/nodes/ESP32-0002/status?isOnline=true"

5. Requirements and local setup

Prerequisites

● Windows 10 or 11 (WPF desktop application requires Windows).

● Visual Studio with the .NET desktop development and ASP.NET and web development workloads, or the corresponding .NET SDK and development tools.

● The .NET SDK version specified by the project’s .csproj files (the project has been developed using .NET 10; verify the checked-in target framework).

● Git and access to the Smart-X repository.

● Postman (optional, for direct endpoint testing).

Clone and restore

git clone <YOUR-GITHUB-REPOSITORY-URL>

cd SmartX-IoT-Mesh-Ecosystem

dotnet restore

Replace the placeholder URL with the actual repository URL. Open the repository’s .sln file in Visual Studio, if present, or open the relevant projects directly.

Start the API

From the repository root, run:

dotnet run --project SmartX.Api --urls http://localhost:5016

Wait for the terminal to report that the API is listening on port 5016. If your project uses a different launch profile or port, update the WPF HttpClient.BaseAddress accordingly.

Start the WPF client

In a second terminal:

dotnet run --project SmartX.Desktop

Alternatively, run SmartX.Api and SmartX.Desktop through Visual Studio using multiple startup projects. Start the API before opening the mesh dashboard.

Navigate the application

1. Open the Smart-X main menu.

2. Select Open Mesh Network.

3. Inspect the node and connection grids and the network graph.

4. Run BFS Traversal, Prim’s MST and Dijkstra Path.

5. In Mesh Fault Simulation, disconnect ESP32-0002, observe rerouting and then reconnect it.

6. In Historical Sensor Telemetry, select a device and a date range containing stored readings, then select Search Historical Telemetry.

6. Verification and demonstration scenarios

|Test                     |Action                                     |Expected outcome                                         |
|-------------------------|-------------------------------------------|---------------------------------------------------------|
|Topology                 |Open mesh dashboard                        |Five seeded nodes, seven connections                     |
|BFS                      |Select BFS Traversal                       |Reachable devices and hop counts are returned            |
|Prim MST                 |Select Prim’s MST while all nodes online   |Four selected links, total weight 8                      |
|Dijkstra                 |Select Dijkstra Path while all nodes online|Cost 8, four hops; current route highlighted             |
|Fault simulation         |Disconnect ESP32-0002                      |Device turns red; attached links become dashed           |
|Automatic reroute        |Inspect Dijkstra result after disconnect   |Alternative route through ESP32-0003, cost 10, three hops|
|Recovery                 |Reconnect ESP32-0002                       |Device turns green; optimal cost-8 route returns         |
|AVL history              |Search a populated date range              |Timestamped readings appear in the history grid          |
|Invalid/unreachable route|Query a missing or unreachable destination |API reports no valid path rather than inventing a route  |

Important: The seed network is a simulation. These tests demonstrate graph algorithms and UI/backend integration, not live radio communication with physical ESP32 hardware.

7. Troubleshooting

The mesh dashboard says “Disconnected” or fails to load. Verify that SmartX.Api is running and listening on http://localhost:5016/, and check that the desktop application’s HttpClient.BaseAddress matches.

A node remains offline after testing. Use Reconnect Device or the corresponding PUT endpoint. Re-run Dijkstra to verify that the original route is restored.

The graph does not highlight the latest route. Confirm that the current successful Dijkstra response updates _highlightedRoute and triggers LoadMeshTopologyAsync() so the canvas is redrawn with the latest path.

Historical telemetry returns no records. Select a device and time range that contains ingested readings. An empty result does not necessarily indicate an AVL tree failure.

The desktop project does not run on macOS/Linux. The UI uses Windows Presentation Foundation (WPF), which is Windows-specific; the API can be run separately on a supported .NET platform.

8. Source code guide

● SmartX.Api/Services/MeshGraphService.cs — graph storage, weighted edges, node status, BFS, Prim and Dijkstra logic.

● SmartX.Desktop/Views/MeshTopologyWindow.xaml — topology, algorithm, recovery and history UI controls.

● SmartX.Desktop/Views/MeshTopologyWindow.xaml.cs — HTTP integration, canvas rendering, route highlighting, fault simulation and history retrieval.

● AVL history service and API endpoint — locate the tree implementation and /api/avl/history mapping in the API project; confirm their exact filenames in the repository before final submission.

● SmartX.Desktop/MainWindow.xaml and MainWindow.xaml.cs — main-menu integration.







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





