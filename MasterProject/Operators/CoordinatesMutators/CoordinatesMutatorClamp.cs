using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static System.Math;

namespace MasterProject.Operators.CoordinatesMutators
{
    public sealed class CoordinatesMutatorClamp<TAlgorithm, TProblem, TSolution>
        : CoordinatesMutator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "C";
        private Func<double[]>? GetLowerSearchBorders { get; set; }
        private Func<double[]>? GetUpperSearchBorders { get; set; }

        public CoordinatesMutatorClamp()
        {
            GetLowerSearchBorders = null;
            GetUpperSearchBorders = null;
        }

        public override CoordinatesMutator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesMutatorClamp<TAlgorithm, TProblem, TSolution>
            {
                GetLowerSearchBorders = (Func<double[]>?) GetLowerSearchBorders?.Clone(),
                GetUpperSearchBorders = (Func<double[]>?) GetUpperSearchBorders?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetLowerSearchBorders = () => algorithm.Problem.LowerSearchBorders;
            GetUpperSearchBorders = () => algorithm.Problem.UpperSearchBorders;
        }

        public override void MutateCoordinates(TSolution[] solutions)
        {
            Assert(GetLowerSearchBorders != null);
            Assert(GetUpperSearchBorders != null);

            double[] lowerSearchBorders = GetLowerSearchBorders();
            double[] upperSearchBorders = GetUpperSearchBorders();


            foreach (var solution in solutions)
            {
                Assert(solution.Coordinates.Length == lowerSearchBorders.Length);
                Assert(solution.Coordinates.Length == upperSearchBorders.Length);

                for (int d = 0; d < solution.Coordinates.Length; d++)
                {
                    solution.Coordinates[d] = Clamp(solution.Coordinates[d], lowerSearchBorders[d],
                                                    upperSearchBorders[d]);
                }
            }
        }
    }
}