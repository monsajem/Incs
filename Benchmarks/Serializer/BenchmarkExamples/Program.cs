using System;
using BenchmarkDotNet.Running;
using System.Collections.Generic;

namespace BenchmarkExample
{
    public class Program
    {
        static void Main(string[] args)
        {
           BenchmarkRunner.Run<Benchmark>();
           Console.ReadLine();
        }
    }
}
