using MasterProject.Solutions;
using static MasterProject.Extensions.ArrayDoubleExtensions;
using static System.Math;
using static System.Diagnostics.Debug;

namespace MasterProject.Problems.Functions
{
    public class Ackley<TSolution>
        : Function<TSolution>
        where TSolution : IPoint<TSolution>
    {
        public Ackley(int dimension)
            : base("Ackley", dimension, TypeOfFunction.Minimization, NewByValue(dimension, 0.0))
        {
        }

        public Ackley(int dimension, double[] lowerSearchBorders, double[] upperSearchBorders)
            : base("Ackley", dimension, TypeOfFunction.Minimization, NewByValue(dimension, 0.0), lowerSearchBorders,
                   upperSearchBorders)
        {
        }

        public override Function<TSolution> DeepClone()
        {
            return new Ackley<TSolution>(Dimension, LowerSearchBorders, UpperSearchBorders);
        }

        public override double GetValueOf(TSolution solution)
        {
            double[] coordinates = solution.Coordinates;

            Assert(Dimension == coordinates.Length);

            double sumSqr = 0.0;
            double sumCos = 0.0;

            foreach (var coordinate in coordinates)
            {
                sumSqr += coordinate * coordinate;
                sumCos += Cos(2.0 * PI * coordinate);
            }

            return -20.0 * Exp(-0.2 * Sqrt(sumSqr / Dimension)) - Exp(sumCos / Dimension) + 20.0 + E;
        }
    }
}