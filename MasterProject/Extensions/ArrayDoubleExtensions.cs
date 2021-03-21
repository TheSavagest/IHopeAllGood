using System;
using static System.Diagnostics.Debug;
using static MasterProject.Extensions.DoubleExtensions;

namespace MasterProject.Extensions
{
    public static class ArrayDoubleExtensions
    {
        public static double[] NewByValue(int size, double value)
        {
            Assert(size > 0);

            var result = new double[size];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = value;
            }

            return result;
        }
        
        public static double[] Copy(this double[] it)
        {
            Assert(it.Length > 0);

            var length = it.Length;
            var result = new double[length];

            Array.Copy(it, result, length);

            return result;
        }

        public static double[] Ranks(double[] it)
        {
            Assert(it.Length > 0);

            var ranks = new double[it.Length];

            for (var i = 0; i < ranks.Length; i++)
            {
                ranks[i] = 1.0 + CountLessThan(it, it[i]) + (CountEquals(it, it[i]) - 1.0) / 2.0;
            }

            return ranks;
        }

        private static int CountLessThan(double[] it, double value)
        {
            Assert(it.Length > 0);

            int count = 0;

            for (int i = 0; i < it.Length; i++)
            {
                if (it[i] < value)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountEquals(double[] it, double value)
        {
            Assert(it.Length > 0);

            var count = 0;

            for (int i = 0; i < it.Length; i++)
            {
                if (IsEqualWithDelta(it[i], value))
                {
                    count++;
                }
            }

            return count;
        }

        public static double Distance(double[] from, double[] to)
        {
            Assert(from.Length == to.Length);

            var distance = 0.0;

            for (var i = 0; i < from.Length; i++)
            {
                distance += (from[i] - to[i]) * (from[i] - to[i]);
            }

            return distance;
        }
    }
}