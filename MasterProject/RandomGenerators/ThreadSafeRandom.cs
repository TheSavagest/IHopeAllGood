using System;
using System.Threading;
using static System.DateTime;
using static System.Diagnostics.Debug;
using static System.Threading.Thread;
using static System.HashCode;

namespace MasterProject.RandomGenerators
{
    public sealed class ThreadSafeRandom : Random
    {
        private ThreadLocal<Random> ThreadLocalRandom { get; }

        public ThreadSafeRandom()
        {
            ThreadLocalRandom = new ThreadLocal<Random>(() => new Random(Combine(CurrentThread.ManagedThreadId,
                                                                                 Now.Ticks)));
        }

        public ThreadSafeRandom(int seed)
        {
            ThreadLocalRandom = new ThreadLocal<Random>(() => new Random(Combine(CurrentThread.ManagedThreadId,
                                                                                 Now.Ticks, seed)));
        }

        public override int Next()
        {
            Assert(ThreadLocalRandom.Value != null);

            return ThreadLocalRandom.Value.Next();
        }

        public override int Next(int maxValue)
        {
            Assert(ThreadLocalRandom.Value != null);

            return ThreadLocalRandom.Value.Next(maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            Assert(ThreadLocalRandom.Value != null);

            return ThreadLocalRandom.Value.Next(minValue, maxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            Assert(ThreadLocalRandom.Value != null);

            ThreadLocalRandom.Value.NextBytes(buffer);
        }

        public override void NextBytes(Span<byte> buffer)
        {
            Assert(ThreadLocalRandom.Value != null);

            ThreadLocalRandom.Value.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            Assert(ThreadLocalRandom.Value != null);

            return ThreadLocalRandom.Value.NextDouble();
        }
    }
}