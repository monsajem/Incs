# Socket Layer Module Documentation

This document explains the structure and functionality of the **Socket** subfolder within the `NetworkService` module of the Incs framework. The folder provides a fully modular, protocol-agnostic socket layer for network communication.

## 1. Exception Handling

**Path:** `NetworkService/Socket/Exceptions/`

* **`SocketException`** and derived classes (`SocketClosedException`, `SocketClosingException`, `ConnectionFailedException`) represent common network errors such as sudden disconnections or connection failures.
* In debug builds, these exceptions capture additional trace information for easier diagnostics.

## 2. Interfaces

**File:** `IClientSocket.cs`

Defines the core contract for a client socket:

```csharp
public interface IClientSocket {
  void Send(byte[] data);
  int Receive(byte[] buffer);
  byte[] Receive(int size);
  void SendPacket(byte[] data);
  byte[] ReceivePacket();
  void Disconnect();
  event Action<Exception> OnError;
}
```

* **Send/Receive Methods:** Support raw byte arrays and length-prefixed packet framing.
* **Disconnect:** Ensures a graceful shutdown with a final handshake.
* **OnError Event:** Notifies subscribers of runtime errors.

## 3. Base Client Implementation

**File:** `ClientSocket.cs`

The `ClientSocket` class centralizes common socket logic while remaining protocol-agnostic.

### 3.1 Connection Lifecycle

* **`Connect(AddressType address)`**: Validates state, invokes `Inner_Connect`, and sets `IsConnected`.
* **`Close()`**: Calls `Inner_Disconnect`, resets state, and clears address.
* **`Disconnect()`**: Waits for pending sends, performs a one-byte handshake, then closes.

### 3.2 Data Transmission

* **`Send(byte[] data)`** / **`_Send(byte[] data)`**

  * Locks connection status and increments in-flight send count.
  * Executes `Inner_Send(data)` alongside a connection-monitor task.
  * Enforces timeouts in debug mode and decrements count on completion.

* **`Receive(byte[] buffer)`** / **`Receive(int size)`**

  * Accumulates incoming bytes in an internal buffer.
  * Awaits buffer changes or disconnection, then copies data out.
  * Throws if disconnected mid-receive.

### 3.3 Packet Framing

* **`SendPacket(byte[] data)`**: Sends a 4-byte length prefix followed by payload.
* **`ReceivePacket()`**: Reads prefix to determine length, then reads exact payload size.

### 3.4 Concurrency and State

* **`Locker<bool> P_IsConnected`**: Thread-safe flag for connection status.
* **`Locker<int> Sendings`**: Tracks active send operations to prevent premature disconnects.
* **`byte[] ReceivedBuffer`**: Internal holding area for incoming data chunks.

### 3.5 Debug and Extension Points

* Debug builds include timeouts and trace logs for troubleshooting.
* **Abstract Methods:**

  * `protected abstract void Inner_Connect(AddressType address);`
  * `protected abstract void Inner_Disconnect();`
  * `protected abstract Task Inner_Send(byte[] data);`

These must be overridden to implement concrete transport protocols (TCP, UDP, in-process channels, SSL/TLS, etc.).

## 4. Protocol-Agnostic Infrastructure

* The socket layer does not assume TCP; transports can use any socket-like medium.
* Common logic (connection management, framing, concurrency) is implemented once in `ClientSocket` and reused across protocols.

## 5. Key Design Principles

1. **Modularity**: Protocol-specific details are confined to `Inner_*` overrides.
2. **Thread-Safety**: Internal lockers prevent race conditions on connection state and send operations.
3. **Graceful Shutdown**: Ensures data integrity via final handshake before closing.
4. **Debug Support**: Enhanced trace and timeout behaviors in debug mode.
