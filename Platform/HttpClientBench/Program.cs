using BenchmarkDotNet.Running;

namespace HttpClientBench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<HttpClientCreator>();
        }
    }
}
