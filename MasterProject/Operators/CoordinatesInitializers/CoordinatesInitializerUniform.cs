using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.CoordinatesInitializers
{
    public sealed class CoordinatesInitializerUniform<TAlgorithm, TProblem, TSolution>
        : CoordinatesInitializer<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>
    {
        public override string SubType => "U";
        private Func<int>? GetDimension { get; set; }
        private Func<double[]>? GetLowerSearchBorders { get; set; }
        private Func<double[]>? GetUpperSearchBorders { get; set; }
        private Func<Random>? GetRandom { get; set; }

        public CoordinatesInitializerUniform()
        {
            GetDimension = null;
            GetLowerSearchBorders = null;
            GetUpperSearchBorders = null;
            GetRandom = null;
        }

        public override CoordinatesInitializer<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesInitializerUniform<TAlgorithm, TProblem, TSolution>
            {
                GetDimension = (Func<int>?) GetDimension?.Clone(),
                GetLowerSearchBorders = (Func<double[]>?) GetLowerSearchBorders?.Clone(),
                GetUpperSearchBorders = (Func<double[]>?) GetUpperSearchBorders?.Clone(),
                GetRandom = (Func<Random>?) GetRandom?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetDimension = () => algorithm.Problem.Dimension;
            GetLowerSearchBorders = () => algorithm.Problem.LowerSearchBorders;
            GetUpperSearchBorders = () => algorithm.Problem.UpperSearchBorders;
            GetRandom = () => algorithm.Random;
        }

        public override void InitializeCoordinates(TSolution[] solutions)
        {
            Assert(GetDimension != null);
            Assert(GetLowerSearchBorders != null);
            Assert(GetUpperSearchBorders != null);
            Assert(GetRandom != null);

            int dimension = GetDimension();
            double[] lowerSearchBorders = GetLowerSearchBorders();
            double[] upperSearchBorders = GetUpperSearchBorders();
            Random random = GetRandom();

            Assert(dimension > 0);    
            Assert(lowerSearchBorders.Length == dimension);    
            Assert(upperSearchBorders.Length == dimension);

            foreach (var solution in solutions)
            {
                solution.Coordinates = new double[dimension];

                for (int d = 0; d < dimension; d++)
                {
                    solution.Coordinates[d] = (upperSearchBorders[d] - lowerSearchBorders[d]) * random.NextDouble()
                                              + lowerSearchBorders[d];
                }
            }
        }
    }
}