using System.Net.Http;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientBench
{
    [MemoryDiagnoser]
    public class HttpClientCreator
    {
        [Benchmark]
        public HttpClient CreateClientEachTime()
        {
            return new HttpClient();
        }

        private readonly static HttpClient _httpClient = new HttpClient();
        [Benchmark]
        public HttpClient CreateStaticClient()
        {
            return _httpClient;
        }

        private static readonly ServiceProvider _serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        [Benchmark]
        public HttpClient CreateClientUsingFactory()
        {
            return _serviceProvider.GetService<IHttpClientFactory>().CreateClient();
        }

        private static readonly HttpClientHandler _handler = new HttpClientHandler();
        [Benchmark]
        public HttpClient CreateClientWithCachedHandler()
        {
            return new HttpClient(_handler);
        }
    }
}
