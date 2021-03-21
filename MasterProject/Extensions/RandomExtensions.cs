using System;
using static System.Diagnostics.Debug;

namespace MasterProject.Extensions
{
    public static class RandomExtensions
    {
        public static int Next(this Random it, double[] probabilities)
        {
            Assert(probabilities.Length > 0);

            double sum = 0.0;
            foreach (double probability in probabilities)
            {
                Assert(probability > 0.0);

                sum += probability;
            }

            double randomValue = it.NextDouble() * sum;
            int index = 0;
            double counter = probabilities[index];

            while (counter < randomValue)
            {
                index++;
                counter += probabilities[index];
            }

            return index;
        }
    }
}