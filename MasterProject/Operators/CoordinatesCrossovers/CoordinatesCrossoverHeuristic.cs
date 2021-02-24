using System;
using MasterProject.Algorithms;
using MasterProject.Extensions;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public sealed class CoordinatesCrossoverHeuristic<TAlgorithm, TProblem, TSolution>
        : CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>
    {
        public override string SubType => "EL";
        private Func<int>? GetDimension { get; set; }
        private Func<Random>? GetRandom { get; set; }

        public CoordinatesCrossoverHeuristic()
        {
            GetDimension = null;
            GetRandom = null;
        }

        public override CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesCrossoverHeuristic<TAlgorithm, TProblem, TSolution>
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

                if (parents[c][1].IsBetterThan(parents[c][0]))
                {
                    for (int d = 0; d < dimension; d++)
                    {
                        children[c].Coordinates[d] = parents[c][1].Coordinates[d] + factor * 
                                                     (parents[c][1].Coordinates[d] - parents[c][0].Coordinates[d]);
                    }
                }
                else
                {
                    for (int d = 0; d < dimension; d++)
                    {
                        children[c].Coordinates[d] = parents[c][0].Coordinates[d] + factor * 
                                                     (parents[c][0].Coordinates[d] - parents[c][1].Coordinates[d]);
                    }
                }
            }
        }
    }
}