using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Platform
{
    public class Multithreading
    {
        private static readonly object _lock = new object();
        public static async Task Lock()
        {
            var criticalResource = new List<string>();
            var tasks = new List<Task>();

            for (int i = 0; i < 500; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    // Some parallel work ...

                    lock (_lock)
                    {
                        // critical resource
                        var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
                        criticalResource.Add(threadId);
                    }

                    // Some parallel work...
                }));
            }

            await Task.WhenAll(tasks);
        }

        public static async Task Monitor()
        {
            var criticalResource = new List<string>();
            var tasks = new List<Task>();

            for (int i = 0; i < 500; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    // Some parallel work ...

                    try
                    {
                        Monitor.Enter(_lock);
                        // critical resource
                        var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
                        criticalResource.Add(threadId);
                    }
                    finally
                    {
                        Monitor.Exit(_lock);
                    }

                    // Some parallel work...
                }));
            }

            await Task.WhenAll(tasks);
        }

        // limits the number of threads that can access a shared resource or a pool of resources concurrently.
        // The state of a semaphore is set to signaled when its count is greater than zero,
        // and nonsignaled when its count is zero.
        public static async Task Semaphore()
        {
            var semaphore = new SemaphoreSlim(5);
            var httpClient = new HttpClient();
            var tasks = new List<Task>();

            for (int i = 0; i < 300; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();

                    // critical section
                    await httpClient.GetAsync("https://google.com");

                    semaphore.Release();
                }));
            }

            await Task.WhenAll(tasks);
        }

        // Grants exclusive access to a shared resource.
        // The state of a mutex is signaled if no thread owns it.
        public static async Task Mutex()
        {
            var mutex = new Mutex();
            var httpClient = new HttpClient();
            var tasks = new List<Task>();

            for (int i = 0; i < 300; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    mutex.WaitOne();

                    // critical section
                    await httpClient.GetAsync("https://google.com");

                    // Only the owner can release the lock 
                    mutex.ReleaseMutex();
                }));
            }

            await Task.WhenAll(tasks);
        }

        // Represents a thread synchronization event and can be either in a signaled or unsignaled state.
        public static void EventWaitHandle()
        {
            var ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        // Derives from EventWaitHandle and, when signaled, resets automatically to an unsignaled state
        // after releasing a single waiting thread.
        public static void AutoResetEvent()
        {
            var re1 = new AutoResetEvent(true);
            var re2 = new AutoResetEvent(false);
        }

        // Derives from EventWaitHandle and, when signaled, stays in a signaled state until the Reset method is called.
        public static void ManualResetEvent()
        {
            // var mre1 = new ManualResetEvent(true);
            var mre = new ManualResetEvent(false);

            new Thread(() =>
            {
                mre.Reset();
                Console.WriteLine("Thread_1: Start doing long job...");
                Thread.Sleep(10_000);
                Console.WriteLine("Thread_1: Long job complited");
                mre.Set();
            }).Start();

            new Thread(() =>
            {
                Console.WriteLine("Thread_2: Waiting...");
                mre.WaitOne();
                Thread.Sleep(2000);
                Console.WriteLine("Thread_2: After waiting complete");
            }).Start();

            // Thread_1: Start doing long job...
            // Thread_2: Waiting...
            // Thread_1: Long job complited
            // Thread_2: After waiting complete
        }

        public static async Task AsyncAwait()
        {
            var tasks = new List<Task<string>>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    return MakeHttpRequest();
                }));
            }

            // Doesn't make an extreme usage of threads but still pretty fast and efficient
            var results = await Task.WhenAll(tasks);
        }

        public static async Task ParallelExample()
        {
            var results = new List<string>();
            var taskSources = new List<Func<string>>();

            for (int i = 0; i < 100; i++)
            {
                taskSources.Add(() => MakeHttpRequest().GetAwaiter().GetResult());
            }

            Parallel.For(0, taskSources.Count, new ParallelOptions
            {
                // By default make an extreme usage of threads and uses all the resources that it can
                MaxDegreeOfParallelism = 2
            },
            (i) =>
            {
                var result = taskSources[i].Invoke();
                results.Add(result);
            });
        }

        private static readonly HttpClient _httpClient = new HttpClient();
        private static async Task<string> MakeHttpRequest()
        {
            var response = await _httpClient.GetAsync("https://google.com");
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
