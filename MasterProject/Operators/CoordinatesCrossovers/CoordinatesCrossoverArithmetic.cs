using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public sealed class CoordinatesCrossoverArithmetic<TAlgorithm, TProblem, TSolution>
        : CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "A";
        private Func<int>? GetDimension { get; set; }
        private Func<Random>? GetRandom { get; set; }

        public CoordinatesCrossoverArithmetic()
        {
            GetDimension = null;
            GetRandom = null;
        }

        public override CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesCrossoverArithmetic<TAlgorithm, TProblem, TSolution>
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

                for (int d = 0; d < dimension; d++)
                {
                    double randomValue = random.NextDouble();
                    children[c].Coordinates[d] = randomValue * parents[c][0].Coordinates[d] +
                                                 (1.0 - randomValue) * parents[c][1].Coordinates[d];
                }
            }
        }
    }
}