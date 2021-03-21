using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.StopConditions
{
    public sealed class MoreEvaluationsThan<TAlgorithm, TProblem, TSolution>
        : StopRule<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        private int MaxEvaluations { get; }
        private Func<int>? GetEvaluations { get; set; }

        public MoreEvaluationsThan(int maxEvaluations)
        {
            MaxEvaluations = maxEvaluations;
            GetEvaluations = null;
        }

        public override StopRule<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new MoreEvaluationsThan<TAlgorithm, TProblem, TSolution>(MaxEvaluations)
            {
                GetEvaluations = (Func<int>?) GetEvaluations?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            GetEvaluations = () => algorithm.Evaluations;
        }

        public override bool IsStop()
        {
            Assert(GetEvaluations != null);

            return GetEvaluations() >= MaxEvaluations;
        }
    }
}