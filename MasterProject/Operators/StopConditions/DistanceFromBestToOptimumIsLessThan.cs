using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static MasterProject.Extensions.ArrayDoubleExtensions;

namespace MasterProject.Operators.StopConditions
{
    public sealed class DistanceFromBestToOptimumIsLessThan<TAlgorithm, TProblem, TSolution>
        : StopRule<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>, new()
    {
        private double Distance { get; }
        private Func<TSolution>? GetBest { get; set; }
        private Func<double[]>? GetOptimalCoordinates { get; set; }

        public DistanceFromBestToOptimumIsLessThan(double distance)
        {
            Distance = distance;
            GetBest = null;
            GetOptimalCoordinates = null;
        }

        public override StopRule<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new DistanceFromBestToOptimumIsLessThan<TAlgorithm, TProblem, TSolution>(Distance)
            {
                GetBest = (Func<TSolution>?) GetBest?.Clone(),
                GetOptimalCoordinates = (Func<double[]>?) GetOptimalCoordinates?.Clone()
            };
        }
        
        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);
            
            GetBest = () => algorithm.Best;
            GetOptimalCoordinates = () => algorithm.Problem.OptimalCoordinates;
        }

        public override bool IsStop()
        {
            Assert(GetBest != null);
            Assert(GetOptimalCoordinates != null);

            return Distance(GetOptimalCoordinates(), GetBest().Coordinates) < Distance;
        }
    }
}