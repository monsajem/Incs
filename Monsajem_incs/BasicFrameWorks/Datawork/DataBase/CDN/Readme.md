# Network Protocol for CDN-Based Database

The CDN-based database is designed to efficiently manage distributed data files using a custom network protocol. This protocol handles file transfers, synchronization, and remote command execution using both TCP and WebSocket connections.

---

## Key Components

- **Network Protocol Implementation:**  
  Located under the KeyValueDatabase interface:  
  [Network Protocol Source](https://github.com/monsajem/Incs/tree/main/Monsajem_incs/BasicFrameWorks/Datawork/DataBase/DatabaseInterface/KeyValueDatabase/Network)  
  This component manages file-based data synchronization and remote instructions. It is responsible for delivering only the updated files (or portions of them) based on the sequential update codes assigned to each record.

- **CDN Main Class:**  
  The entry point for CDN-based database operations is implemented in the CDN class:  
  [CDN.cs](https://github.com/monsajem/Incs/blob/main/Monsajem_incs/BasicFrameWorks/Datawork/DataBase/CDN/CDN.cs)  
  This class integrates the network protocol with the main database logic. It ensures that clients receive only the necessary updates (based on the update code) and manages file-level data distribution to improve load times and reduce bandwidth.

---

## Key Features

- **Efficient Data Synchronization:**  
  Transfers only the records or files with a higher update code than the client’s current state, thereby optimizing bandwidth and reducing unnecessary data transfer.

- **File-Based Distribution:**  
  Data is distributed across multiple files based on relationships between records. Clients load only the required segments rather than the entire database.

- **Dual Connection Support:**  
  The protocol supports both TCP and WebSocket connections, providing flexibility for real-time and request-based synchronization.

- **Remote Command Execution:**  
  The network layer allows remote execution of commands such as triggering updates, file integrity checks, and other administrative tasks.

---

## How It Works

1. **Update Code Mechanism:**  
   Each record in the database carries a sequential update code. When a client connects, it uses this code to request only the newer records/files from the server.

2. **File-Level Synchronization:**  
   The network protocol groups related records into files. When updates occur, only the affected files are transferred, making the synchronization process both fast and efficient.

3. **Integration with CDN.cs:**  
   The `CDN.cs` class serves as the core controller for the CDN-based database, using the network protocol to fetch updates and manage the distributed data files.

---

## Example Workflow

1. A client initiates a connection via TCP or WebSocket.
2. The client sends its current update code to the server.
3. The network protocol checks for files with records that have a higher update code.
4. Only the updated files are sent to the client.
5. The client integrates the received updates into its local session-based or disk-based storage.

This architecture ensures that clients always work with the latest data without needing to reload the entire database, significantly improving performance and reducing network overhead.
