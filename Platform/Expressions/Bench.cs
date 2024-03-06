using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace HighPerformance
{
    [MemoryDiagnoser]
    public class Bench
    {
        // [Params(1, 2, 3)]
        //public int Size { get; set; }

        // [GlobalSetup]
        //public void Setup()
        //{
        //    // Do some setup
        //}

        private static readonly string _date = "2022 12 22";
        private static readonly int _arraySize = 2000;

        [Benchmark]
        public DateTime UsingMemory()
        {
            var dateAsMemory = _date.AsMemory();

            var res = Parse(dateAsMemory.Span);

            return res;
        }

        [Benchmark]
        public DateTime UsingSpan()
        {
            var dateAsSpan = _date.AsSpan();

            var res = Parse(dateAsSpan);

            return res;
        }

        [Benchmark]
        public string WorkerCreatingArrayEachTime()
        {
            // 10
            var res = WorkerWithBuffer.Process(_arraySize);

            return res;
        }

        [Benchmark]
        public string WorkerUsingArrayPool()
        {
            // 10
            var res = WorkerWithBuffer.ProcessUsingArrayPool(_arraySize);

            return res;
        }

        public static DateTime Parse(ReadOnlySpan<char> date)
        {
            var year = date.Slice(0, 4);
            var month = date.Slice(5, 2);
            var day = date.Slice(8, 2);

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
    }
}
