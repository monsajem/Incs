# <img src="https://github.com/monsajem/Logo_files/blob/master/MonsajemLogo.png" width="50" height="25"> Monsajem Soft incs

A complete and modular framework for building all kinds of software applications — featuring a high-performance custom database, a custom communication protocol, and a modern Blazor-based UI layer.

This repository is organized into multiple components, each serving a specific purpose in the framework's ecosystem. From low-level data manipulation and serialization to full-stack UI, everything is built from the ground up for performance and flexibility.

---

## Key Features

- **Custom Database Engine**  
  10x faster than LiteDB, crash-resilient, no fragmentation, and optimized for disk and memory.

- **Custom Network Protocol**  
  Lightweight and efficient communication protocol for high-performance distributed systems.

- **Blazor-Based UI Framework**  
  Build reactive, component-based UIs using .NET Blazor.

- **Advanced Data Structures**  
  Including AVL trees, low-level array interfaces, and unsafe memory operations.

---

## Framework Modules

Here are the main modules included in this framework:

- **[Data Handling](./Monsajem_incs/BasicFrameWorks/Datawork)**  
  Custom data structures, type converters, and binary serializers optimized for storage and performance.

- **[Networking](./Monsajem_incs/BasicFrameWorks/Network)**  
  Lightweight protocol for sending and receiving structured data efficiently, along with services and infrastructure for data exchange and remote command execution.

- **[Database](./Monsajem_incs/BasicFrameWorks/Datawork/Database)**  
  A custom, persistent, and fast NoSQL-style storage engine.

- **[Benchmarks & Tests](./Tests)**  
  Performance evaluations and automated tests to ensure stability.

---

## Getting Started

To clone and build the project:

```bash
git clone https://github.com/monsajem/Incs.git
cd Incs
dotnet build
