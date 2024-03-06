using System;
using System.Buffers;
using System.Text;

namespace HighPerformance
{
    public class WorkerWithBuffer
    {
        private static readonly Random _random = new Random();

        public static string Process(int arraySize)
        {
            var buffer = new int[arraySize];

            var sb = new StringBuilder();

            for (int i = 0; i < buffer.Length; i++)
            {
                // Some work with buffer
                buffer[i] = _random.Next();

                sb.Append(buffer[i]);
            }

            return sb.ToString();
        }

        public static string ProcessUsingArrayPool(int arraySize)
        {
            var buffer = ArrayPool<int>.Shared.Rent(arraySize);

            var sb = new StringBuilder();

            for (int i = 0; i < arraySize; i++)
            {
                // Some work with buffer
                buffer[i] = _random.Next();

                sb.Append(buffer[i]);
            }

            ArrayPool<int>.Shared.Return(buffer);

            return sb.ToString();
        }
    }
}
