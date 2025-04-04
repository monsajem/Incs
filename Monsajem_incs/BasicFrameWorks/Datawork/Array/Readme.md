# Monsajem IArray

An advanced and flexible array framework centered around the `IArray` interface — designed to standardize and extend array behavior beyond .NET’s built-in capabilities.

This module provides a unified structure for array-like types, supporting a wide range of features like dynamic resizing, optimized insert/remove operations, and memory/storage abstraction.

---

## Key Concepts

- **IArray Interface**  
  A unified interface that defines core operations such as insert, remove, shift, and more. All array types in this module implement `IArray`.

- **Multiple Array Models**  
  Several optimized array implementations, all based on `IArray`, designed for different use-cases and performance goals:
  - **Tree-based Arrays**  
    Reduce the overhead of inserting/removing elements in the middle of large arrays.
  - **Disk-backed Arrays**  
    Arrays that persist their content directly to hard drive, enabling storage of massive datasets.
  - **Stream-based Arrays**  
    Designed to read/write from streams with minimal memory footprint.

---

## Features

- Standardized operations across all array types  
- Fast insert/delete/shift even in large collections  
- Pluggable storage backends: memory, disk, or stream  
- Designed for high performance and low GC pressure  
- Easily extendable with custom implementations

---

## Use Cases

- Real-time data manipulation  
- Persistent array structures  
- Efficient large data processing (e.g., logs, datasets, file-based indexes)  
- In-memory or hybrid (RAM + disk) solutions

---

## Example

Below is an example using the tree-based array implementation from `Monsajem_Incs.Collection.Array.TreeBased`:

```csharp
using Monsajem_Incs.Collection.Array.TreeBased;
using System;

class Program
{
    static void Main()
    {
        // Create a tree-based IArray
        var numbers = new Array<int>();

        numbers.Insert(0, 10);
        numbers.Insert(1, 20);
        numbers.Insert(1, 15); // inserts 15 between 10 and 20

        Console.WriteLine(string.Join(", ", numbers.ToArray()));
        // Expected Output: 10, 15, 20

        numbers.Remove(1); // removes 15
        Console.WriteLine(string.Join(", ", numbers.ToArray()));
        // Expected Output: 10, 20
    }
}
