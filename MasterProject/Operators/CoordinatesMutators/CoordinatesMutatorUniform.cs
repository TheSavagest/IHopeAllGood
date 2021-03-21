using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesMutators
{
    public sealed class CoordinatesMutatorUniform<TAlgorithm, TProblem, TSolution>
        : CoordinatesMutator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "U";
        private Func<double[]>? GetLowerSearchBorders { get; set; }
        private Func<double[]>? GetUpperSearchBorders { get; set; }
        private Func<Random>? GetRandom { get; set; }
        private double MutationProbability { get; }

        public CoordinatesMutatorUniform(double mutationProbability = 0.05)
        {
            Assert(mutationProbability > 0.0);
            Assert(mutationProbability < 1.0);

            GetLowerSearchBorders = null;
            GetUpperSearchBorders = null;
            GetRandom = null;
            MutationProbability = mutationProbability;
        }

        public override CoordinatesMutator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesMutatorUniform<TAlgorithm, TProblem, TSolution>(MutationProbability)
            {
                GetLowerSearchBorders = (Func<double[]>?) GetLowerSearchBorders?.Clone(),
                GetUpperSearchBorders = (Func<double[]>?) GetUpperSearchBorders?.Clone(),
                GetRandom = (Func<Random>?) GetRandom?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetLowerSearchBorders = () => algorithm.Problem.LowerSearchBorders;
            GetUpperSearchBorders = () => algorithm.Problem.UpperSearchBorders;
            GetRandom = () => algorithm.Random;
        }

        public override void MutateCoordinates(TSolution[] solutions)
        {
            Assert(GetLowerSearchBorders != null);
            Assert(GetUpperSearchBorders != null);
            Assert(GetRandom != null);

            double[] lowerSearchBorders = GetLowerSearchBorders();
            double[] upperSearchBorders = GetUpperSearchBorders();
            Random random = GetRandom();

            foreach (var solution in solutions)
            {
                Assert(solution.Coordinates.Length == lowerSearchBorders.Length);
                Assert(solution.Coordinates.Length == upperSearchBorders.Length);

                for (int d = 0; d < solution.Coordinates.Length; d++)
                {
                    if (random.NextDouble() < MutationProbability)
                    {
                        solution.Coordinates[d] = lowerSearchBorders[d] + random.NextDouble() *
                                                  (upperSearchBorders[d] - lowerSearchBorders[d]);
                    }
                }
            }
        }
    }
}