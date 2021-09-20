using System;
using BenchmarkDotNet.Running;
using System.Collections.Generic;

namespace BenchmarkExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var x = new Benchmark();
            x.GlobalS();
            x.Class();
           BenchmarkRunner.Run<Benchmark>();
           Console.ReadLine();
        }
    }
}
