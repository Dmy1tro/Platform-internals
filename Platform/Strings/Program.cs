using System;
using BenchmarkDotNet.Running;

namespace Strings
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Bench>();
        }
    }
}
