using System;
using static System.Diagnostics.Debug;

namespace MasterProject.Extensions
{
    public static class ArrayIntExtensions
    {
        //https://stackoverflow.com/questions/1608181/unique-random-numbers-in-an-integer-array-in-the-c-programming-language
        public static int[] Unique(int count, int from, int to, Random random)
        {
            Assert(count > 0);
            Assert(to - from > count);

            var unique = new int[count];
            int it = 0;
            int ip = 0;

            while (ip < to && it < count)
            {
                int rp = to - ip;
                int rt = count - it;

                if (random.Next() % rp < rt)
                {
                    unique[it] = ip + from;
                    it++;
                }

                ip++;
            }

            return unique;
        }
    }
}