using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public sealed class CoordinatesCrossoverProportional<TAlgorithm, TProblem, TSolution>
        : CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "EL";
        private Func<int>? GetDimension { get; set; }

        public CoordinatesCrossoverProportional()
        {
            GetDimension = null;
        }

        public override CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesCrossoverProportional<TAlgorithm, TProblem, TSolution>()
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
                double fit1 = parents[c][0].Fitness;
                double fit2 = parents[c][1].Fitness;
                double sumFit = fit1 + fit2;
                fit1 /= sumFit;
                fit2 /= sumFit;

                for (var d = 0; d < dimension; d++)
                {
                    children[c].Coordinates[d] = fit1 * parents[c][0].Coordinates[d] +
                                                 fit2 * parents[c][1].Coordinates[d];
                }
            }
        }
    }
}