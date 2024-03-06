using System;
using System.Threading;
using System.Threading.Tasks;

namespace VolatileExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Example();
        }

        public static void Example()
        {
            Console.WriteLine("The start");
            var worker = new Worker();
            var thread = new Thread(() =>
            {
                worker.TurnOnPower();
                worker.DoJob();
            });

            Console.WriteLine("Thread start");
            thread.Start();

            while (!thread.IsAlive)
            {
                Task.Delay(100).GetAwaiter().GetResult();
            }

            Task.Delay(700).GetAwaiter().GetResult();
            Thread.Sleep(500);

            Console.WriteLine("Turn off power");
            worker.TurnOffPower();

            Console.WriteLine("Thread join");
            thread.Join();

            Console.WriteLine("The end");
        }
    }

    class Worker
    {
        private bool powerOn = false;
        private int number = 0;

        public void TurnOnPower()
        {
            powerOn = true;
        }

        public void TurnOffPower()
        {
            Volatile.Write(ref powerOn, false);
            Interlocked.Increment(ref number);
            Interlocked.Decrement(ref number);

            powerOn = false;
        }

        public void DoJob()
        {
            var ticks = 0;
            while (powerOn)
            {
                ticks++;
                Task.Delay(10).GetAwaiter().GetResult();
            }

            Console.WriteLine($"Ticks => {ticks}");
            Console.WriteLine("Power has been turned off");
        }
    }
}
