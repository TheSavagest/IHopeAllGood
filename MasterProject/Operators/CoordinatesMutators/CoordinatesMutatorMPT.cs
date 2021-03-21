using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static System.Math;

namespace MasterProject.Operators.CoordinatesMutators
{
    public sealed class CoordinatesMutatorMPT<TAlgorithm, TProblem, TSolution>
        : CoordinatesMutator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "C";
        private Func<double[]>? GetLowerSearchBorders { get; set; }
        private Func<double[]>? GetUpperSearchBorders { get; set; }
        private Func<Random>? GetRandom { get; set; }
        private double MutationProbability { get; }
        private double Power { get; }

        public CoordinatesMutatorMPT(double mutationProbability = 0.05, double power = 3)
        {
            Assert(mutationProbability > 0.0);
            Assert(mutationProbability < 1.0);
            Assert(power > 0.0);

            GetLowerSearchBorders = null;
            GetUpperSearchBorders = null;
            GetRandom = null;
            MutationProbability = mutationProbability;
            Power = power;
        }

        public override CoordinatesMutator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesMutatorMPT<TAlgorithm, TProblem, TSolution>(MutationProbability, Power)
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
                        double t = (solution.Coordinates[d] - lowerSearchBorders[d]) /
                                   (upperSearchBorders[d] - lowerSearchBorders[d]);
                        double r = random.NextDouble();
                        double tt = r.CompareTo(t) switch
                        {
                            -1 => t - t * Pow((t - r) / t, Power),
                            1 => t + (1 - t) * Pow((r - t) / (1 - t), Power),
                            _ => t
                        };

                        solution.Coordinates[d] = (1 - tt) * lowerSearchBorders[d] + tt * upperSearchBorders[d];
                    }
                }
            }
        }
    }
}