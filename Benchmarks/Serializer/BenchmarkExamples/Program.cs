using System;
using BenchmarkDotNet.Running;
using System.Collections.Generic;

namespace BenchmarkExample
{
    public struct str
    {
        public fixed
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var x = new Benchmark();
            x.GlobalS();
            x.Integer();
           BenchmarkRunner.Run<Benchmark>();
           Console.ReadLine();
        }
    }
}
