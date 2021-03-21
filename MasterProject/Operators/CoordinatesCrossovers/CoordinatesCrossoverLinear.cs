using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public sealed class CoordinatesCrossoverLinear<TAlgorithm, TProblem, TSolution>
        : CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "EL";
        private bool IsCentred { get; }
        private Func<int>? GetDimension { get; set; }

        public CoordinatesCrossoverLinear(bool isCentred)
        {
            IsCentred = isCentred;
            GetDimension = null;
        }

        public override CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesCrossoverLinear<TAlgorithm, TProblem, TSolution>(IsCentred)
            {
                GetDimension = (Func<int>?) GetDimension?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetDimension = () => algorithm.Problem.Dimension;
        }

        public override void CrossCoordinates(TSolution[][] parents, TSolution[] children)
        {
            Assert(parents.Length == children.Length);
            Assert(GetDimension != null);

            int dimension = GetDimension();

            Assert(dimension > 0);

            for (int c = 0; c < children.Length; c++)
            {
                Assert(parents[c].Length == 2);
                Assert(dimension == parents[c][0].Coordinates.Length);
                Assert(dimension == parents[c][1].Coordinates.Length);

                children[c].Coordinates = new double[dimension];

                if (IsCentred)
                {
                    for (int d = 0; d < dimension; d++)
                    {
                        children[c].Coordinates[d] = 0.5 * parents[c][0].Coordinates[d] +
                                                     0.5 * parents[c][1].Coordinates[d];
                    }
                }
                else
                {
                    for (int d = 0; d < dimension; d++)
                    {
                        children[c].Coordinates[d] = 1.5 * parents[c][0].Coordinates[d] -
                                                     0.5 * parents[c][1].Coordinates[d];
                    }
                }
            }
        }
    }
}