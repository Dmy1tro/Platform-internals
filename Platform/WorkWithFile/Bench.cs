using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace WorkWithFile
{
    [MemoryDiagnoser]
    public class Bench
    {
        //[Benchmark]
        //public void ReadAndProcessInParallel()
        //{
        //    FileWorker.ReadAndProcessInParallel();
        //}

        [Benchmark]
        public async Task ReadAndProccessConsistently()
        {
            await FileWorker.ReadAndProcessConsistently();
        }

        //[Benchmark]
        //public void ReadAndProcessInParallel_v2()
        //{
        //    FileWorker.ReadAndProcessInParallel_v2();
        //}

        [Benchmark]
        public async Task ReadAndProcessInParallel_v3()
        {
            await FileWorker.ReadAndProcessInParallel_V3();
        }
    }
}
