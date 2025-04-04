# ConvertorFromTo

The **ConvertorFromTo** class simplifies data type conversions by allowing you to define custom conversion logic between two types. Once a conversion is registered, it can be used throughout the project. This reduces code duplication and ensures consistent transformation of data types.

## Example Usage

### Registering a Custom Converter
To define a custom converter between types (e.g., `string` to `int`), you can register a conversion function using the `Register<TFrom, TTo>` method.

```csharp
using Monsajem_Incs.BasicFrameWorks.Datawork.Convertors;

class Program
{
    static void Main()
    {
        // Register a custom conversion from string to int
        ConvertorFromTo.Register<string, int>(s => int.Parse(s));
        
        // Use the conversion
        string input = "456";
        int result = ConvertorFromTo.Convert<string, int>(input);
        
        Console.WriteLine($"Converted '{input}' to {result}");
    }
}
