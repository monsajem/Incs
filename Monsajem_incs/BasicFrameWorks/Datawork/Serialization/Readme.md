
# Monsajem Serializer

A high-performance, secure, and easy-to-use serializer designed to efficiently convert any .NET data type into a compact byte representation — and back.

This serializer is built from scratch with a focus on:

- **Speed**: Optimized for rapid serialization and deserialization of both simple and complex types.
- **Safety**: Handles edge cases and invalid data safely, with careful memory access and validation.
- **Ease of Use**: Minimal setup required. Supports automatic serialization of most .NET types out of the box.

---

## Features

- Supports primitive types, arrays, collections, objects, and custom types  
- Memory-efficient encoding  
- Minimal allocations and GC pressure  
- Handles circular references and deep object graphs  
- Easily extendable for custom data types  

---

## Use Cases

Ideal for:

- Persisting data structures to disk  
- Network transmission of structured data  
- High-performance caching systems  

---

## Example

```csharp
using Monsajem_Incs.Serialization;
using System;

public class Person
{
    public string Name;
    public int Age;
}

class Program
{
    static void Main()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        // Serialize to byte array
        var serialized = Serializer.Serialize(person);

        // Deserialize back to object
        var deserialized = Serializer.Deserialize<Person>(serialized);

        Console.WriteLine($"Name: {deserialized.Name}, Age: {deserialized.Age}");
    }
}
