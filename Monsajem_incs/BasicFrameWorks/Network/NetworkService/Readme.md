# NetworkService Module

The **NetworkService** module in the Incs framework provides a pluggable, transport-agnostic abstraction layer for building both client–server and peer-to-peer networked applications. It centralizes the core networking logic—connection management, message serialization, transport configuration, and event-driven communication—into a set of well-defined interfaces and implementations.

## Table of Contents

1. [Overview](#overview)
2. [Key Concepts](#key-concepts)
3. [Core Interfaces](#core-interfaces)
4. [Primary Implementations](#primary-implementations)
5. [Configuration & Dependency Injection](#configuration--dependency-injection)
6. [Usage Example](#usage-example)
7. [Extensibility](#extensibility)

## Overview

The NetworkService module decouples transport-level details (TCP, HTTP, WebSocket, etc.) from application logic. By programming against interfaces such as `INetworkService` and `INetworkClient`, developers can swap or mock underlying protocols without modifying business code. Typical responsibilities include:

* Lifecycle management (`Start()`, `Stop()`, `Dispose()`)
* Connection handling (`Connect()`, `Disconnect()`)
* Message dispatch (`Send<T>()`, `Broadcast<T>()`)
* Event-based message receipt (via callbacks or events)
* Pluggable serialization strategies

## Key Concepts

### Transport Agnosticism

The module treats the network layer as an interchangeable component. You can plug in TCP sockets, HTTP long-polling, or even custom transports.

### Type-Safe Messaging

All messages are strongly typed generic payloads. The framework ensures at compile-time that only supported types are sent and received.

### Event-Driven Architecture

Incoming messages trigger events or callback handlers, enabling asynchronous, non-blocking workflows.

## Core Interfaces

```csharp
public interface INetworkService : IDisposable {
    void Start();
    void Stop();
    void Broadcast<T>(T payload);
    void Send<T>(string targetId, T payload);
    event Action<string, byte[]> OnRawMessage;
}

public interface INetworkClient {
    string Id { get; }
    void Connect(string address, int port);
    void Disconnect();
    void Send<T>(T payload);
    event Action<byte[]> OnMessage;
}
```

## Primary Implementations

* **NetworkService**: Listens for incoming connections, manages client registry, and routes messages.
* **NetworkClient**: Establishes outbound connections and handles reconnection logic.
* **SocketTransport**: A basic TCP socket-based transport layer.
* **HttpTransport**: Uses HTTP requests for send/receive in environments where sockets may be restricted.

## Configuration & Dependency Injection

Register services in your IoC container:

```csharp
services.AddSingleton<INetworkService, NetworkService>();
services.AddTransient<INetworkClient, NetworkClient>();
```

Configure via `appsettings.json`:

```json
{
  "NetworkSettings": {
    "Port": 9000,
    "MaxConnections": 100,
    "Transport": "Socket"
  }
}
```

## Usage Example

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services) {
    services.AddNetworkService(Configuration.GetSection("NetworkSettings"));
}

// In application code
var netService = serviceProvider.GetRequiredService<INetworkService>();
netService.OnRawMessage += (senderId, data) => {
    var msg = MyMessage.Deserialize(data);
    Console.WriteLine($"Received from {senderId}: {msg.Text}");
};
netService.Start();

// Sending a message\ netService.Broadcast(new MyMessage { Text = "Hello peers!" });
```

## Extensibility

* **Custom Transports**: Implement `ITransport` to integrate new protocols.
* **Custom Serializers**: Swap JSON for Protobuf or your secure, adaptive serializer by implementing `ISerializer`.
* **Testing & Mocks**: Mock `INetworkClient` in unit tests to simulate remote peers.

---

*End of NetworkService documentation.*
