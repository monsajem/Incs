# Async Operations Module Documentation

This document describes the `Oprations_Async.cs` file in the `NetworkService/Service` folder of the Incs framework. It defines the asynchronous operations layer that powers RPC-style communication over the underlying socket transport.

## Overview

The `Oprations_Async.cs` file provides:

* **Attributes**: Markers to designate remote-callable and synchronized members.
* **`IAsyncOprations<T>` interface**: Defines all async network operations and RPC methods.
* **`AsyncOprations<T>` class**: Implements the interface using the `ClientSocket`, handling send/receive, framing, parity checks, and dynamic remote invocation via reflection.

## 1. Attributes

* **`[Remotable]`**: Applied to delegate fields to indicate they should be callable remotely.
* **`[Syncable]`**: Marks fields whose invocations require parity synchronization between peers.

These attributes guide the dynamic setup of RPC handlers and synchronization logic.

## 2. `IAsyncOprations<T>` Interface

```csharp
public interface IAsyncOprations<T>
{
    Task<T> SendData(T data);
    Task<T> GetData();

    Task SendArray(IEnumerable<T> datas, Action<T> onSent = null);
    Task GetArray(Action<T> onReceived);
    Task<T[]> GetArray();

    Task RunOnOtherSide(Func<Task> action);
    Task<TResult> RunOnOtherSide<TResult>(Func<Task<TResult>> func);
    Task RunRecivedAction(Action<T> permission, DataType data);

    Task Sync();
    Task Sync(Action action);
    Task Sync(Action<T> action);

    Task Stop();
}
```

* **Data Operations**: Send/receive single items, arrays, and callbacks.
* **Remote Invocation**: `RunOnOtherSide` methods let you send and execute delegates on the peer.
* **Synchronization**: `Sync` methods exchange parity signals to maintain ordering and detect mismatches.
* **Lifecycle**: `Stop()` cleanly shuts down operations.

## 3. `AsyncOprations<T>` Implementation

### 3.1 Core Fields

* `ClientSocket Client`: Underlying socket for byte-level I/O.
* `bool IsServer`: Indicates whether this side treats parity as server or client.

### 3.2 Sending and Receiving Data

* **`SendData`**:

  * Serializes `T` into bytes.
  * Chooses packet framing if size unknown or length-prefix otherwise.
  * In debug builds, performs parity checks before/after sending.

* **`GetData`**:

  * Reads next packet or raw bytes.
  * Deserializes into `T`.
  * On server side, raises a `Mistake` event upon deserialization errors.

### 3.3 Array Operations

* **`SendArray`** / **`GetArray`**: Iteratively send or receive multiple `T` items, invoking callbacks as items arrive.

### 3.4 Remote Delegate Invocation

* **`Remote` Setup**:

  * Reflects over `[Remotable]` and `[Syncable]` delegate fields.
  * Wraps each delegate so that invoking it serializes its arguments, sends to peer, and awaits the response.

* **`RunRecivedAction`**:

  * Deserializes an incoming delegate invocation request.
  * Executes the corresponding local delegate and sends back the result or error.

### 3.5 Parity Synchronization (Debug Only)

* **`ClientParity`** and **`ServerParity`** methods exchange small parity packets to ensure the sequence of send/get calls remains consistent. Mismatches lead to immediate shutdown and exceptions.

### 3.6 Shutdown

* **`Stop`**: Invokes `Client.Disconnect()` for a graceful close.

## 4. Design Considerations

1. **Protocol-Agnostic**: Relies solely on `ClientSocket` for transport, allowing any socket-like medium.
2. **Reflection-Based RPC**: Automates delegate publishing and invocation without manual stub generation.
3. **Built-In Parity Checks**: Helps catch out-of-order or dropped messages during development.
4. **Comprehensive Async API**: Supports varied communication patterns (unary, callbacks, arrays, RPC) with a single interface.

---

*End of Async Operations documentation.*
