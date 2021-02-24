using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static System.Math;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public sealed class CoordinatesCrossoverBLX<TAlgorithm, TProblem, TSolution>
        : CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>
    {
        public override string SubType => "BLX";
        private Func<int>? GetDimension { get; set; }
        private Func<Random>? GetRandom { get; set; }

        public CoordinatesCrossoverBLX()
        {
            GetDimension = null;
            GetRandom = null;
        }

        public override CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesCrossoverBLX<TAlgorithm, TProblem, TSolution>
            {
                GetDimension = (Func<int>?) GetDimension?.Clone(),
                GetRandom = (Func<Random>?) GetRandom?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetDimension = () => algorithm.Problem.Dimension;
            GetRandom = () => algorithm.Random;
        }

        public override void CrossCoordinates(TSolution[][] parents, TSolution[] children)
        {
            Assert(parents.Length == children.Length);
            Assert(GetDimension != null);
            Assert(GetRandom != null);

            int dimension = GetDimension();
            Random random = GetRandom();

            Assert(dimension > 0);
            
            for (int c = 0; c < children.Length; c++)
            {
                Assert(parents[c].Length == 2);
                Assert(dimension == parents[c][0].Coordinates.Length);
                Assert(dimension == parents[c][1].Coordinates.Length);

                children[c].Coordinates = new double[dimension];
                double factor = random.NextDouble();

                for (int d = 0; d < dimension; d++)
                {
                    double l = Min(parents[c][0].Coordinates[d], parents[c][1].Coordinates[d]);
                    double u = Max(parents[c][0].Coordinates[d], parents[c][1].Coordinates[d]);
                    double r = u - l;
                    r *= factor;
                    l -= r;
                    u += r;

                    children[c].Coordinates[d] = (u - l) * random.NextDouble() + l;
                }
            }
        }
    }
}