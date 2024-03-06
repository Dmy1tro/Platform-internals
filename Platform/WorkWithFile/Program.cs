using BenchmarkDotNet.Running;

namespace WorkWithFile
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Bench>();
        }
    }
}
