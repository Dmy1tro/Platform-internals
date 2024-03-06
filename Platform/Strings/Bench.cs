using System;
using BenchmarkDotNet.Attributes;

namespace Strings
{
    [MemoryDiagnoser]
    public class Bench
    {
        private static readonly string _date = "2022 12 22";

        [Benchmark]
        public DateTime ParseDateWithSubstring()
        {
            return DateParser.Parse(_date);
        }

        [Benchmark]
        public DateTime ParseDateUsingSpan()
        {
            return DateParser.ParseOptimized(_date);
        }

        [Benchmark]
        public DateTime ParseDateUsingSpanAndStackalloc()
        {
            return DateParser.ParseWithStackAllocation(_date);
        }
    }
}
