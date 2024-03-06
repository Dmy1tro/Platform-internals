using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WorkWithFile
{
    public class FileWorker
    {
        public static void ReadAndProcessInParallel()
        {
            var pathes = new string[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "resource1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource2.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource3.txt")
            };

            using var lines = new BlockingCollection<string>();

            var readTask1 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[0]))
                {
                    lines.Add(line);
                }
            });

            var readTask2 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[1]))
                {
                    lines.Add(line);
                }
            });

            var readTask3 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[2]))
                {
                    lines.Add(line);
                }
            });

            var readTasks = Task.WhenAll(readTask1, readTask2, readTask3).ContinueWith(_ =>
            {
                lines.CompleteAdding();
            });

            var processTask = Task.Run(() =>
            {
                var numbers = new List<double>();

                foreach (var line in lines.GetConsumingEnumerable())
                {
                    numbers.Add(double.Parse(line));
                }

                return numbers;
            });

            Task.WhenAll(readTasks, processTask).GetAwaiter().GetResult();
        }

        public static async Task ReadAndProcessConsistently()
        {
            var pathes = new string[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "resource1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource2.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource3.txt")
            };

            var lines1 = await File.ReadAllLinesAsync(pathes[0]);
            var lines2 = await File.ReadAllLinesAsync(pathes[1]);
            var lines3 = await File.ReadAllLinesAsync(pathes[2]);
            var numbers = new List<double>();

            foreach (var line in lines1)
            {
                numbers.Add(double.Parse(line));
            }

            foreach (var line in lines2)
            {
                numbers.Add(double.Parse(line));
            }

            foreach (var line in lines3)
            {
                numbers.Add(double.Parse(line));
            }
        }

        public static void ReadAndProcessInParallel_v2()
        {
            var pathes = new string[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "resource1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource2.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource3.txt")
            };

            using var resource1 = new BlockingCollection<string>();
            using var resource2 = new BlockingCollection<string>();
            using var resource3 = new BlockingCollection<string>();

            var readResource1 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[0]))
                {
                    resource1.Add(line);
                }

                resource1.CompleteAdding();
            });

            var readResource2 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[1]))
                {
                    resource2.Add(line);
                }

                resource2.CompleteAdding();
            });

            var readResource3 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[2]))
                {
                    resource3.Add(line);
                }

                resource3.CompleteAdding();
            });

            var processResource1 = Task.Run(() =>
            {
                var numbers = new List<double>();

                foreach (var line in resource1.GetConsumingEnumerable())
                {
                    numbers.Add(double.Parse(line));
                }

                return numbers;
            });

            var processResource2 = Task.Run(() =>
            {
                var numbers = new List<double>();

                foreach (var line in resource2.GetConsumingEnumerable())
                {
                    numbers.Add(double.Parse(line));
                }

                return numbers;
            });

            var processResource3 = Task.Run(() =>
            {
                var numbers = new List<double>();

                foreach (var line in resource3.GetConsumingEnumerable())
                {
                    numbers.Add(double.Parse(line));
                }

                return numbers;
            });

            Task.WhenAll(readResource1, readResource2, readResource3, processResource1, processResource2, processResource3)
                .GetAwaiter().GetResult();
        }

        public static async Task ReadAndProcessInParallel_V3()
        {
            var pathes = new string[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "resource1.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource2.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "resource3.txt")
            };

            var numbers = new ConcurrentBag<double>();

            var processTask1 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[0]))
                {
                    numbers.Add(double.Parse(line));
                }
            });

            var processTask2 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[1]))
                {
                    numbers.Add(double.Parse(line));
                }
            });

            var processTask3 = Task.Run(() =>
            {
                foreach (var line in File.ReadLines(pathes[2]))
                {
                    numbers.Add(double.Parse(line));
                }
            });

            await Task.WhenAll(processTask1, processTask2, processTask3);
        }
    }
}
