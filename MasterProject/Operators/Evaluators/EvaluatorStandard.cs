using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static System.Threading.Interlocked;

namespace MasterProject.Operators.Evaluators
{
    public sealed class EvaluatorStandard<TAlgorithm, TProblem, TSolution>
        : Evaluator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "S";
        private Func<TSolution, double>? ValueFunction { get; set; }

        public EvaluatorStandard()
        {
            ValueFunction = null;
        }

        public override Evaluator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new EvaluatorStandard<TAlgorithm, TProblem, TSolution>
            {
                EvaluationCounter = EvaluationCounter,
                ValueFunction = (Func<TSolution, double>?) ValueFunction?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            ValueFunction = solution => algorithm.Problem.GetValueOf(solution);
        }

        public override void Evaluate(TSolution[] solutions)
        {
            Assert(ValueFunction != null);

            foreach (var solution in solutions)
            {
                solution.Value = ValueFunction(solution);
            }

            Add(ref EvaluationCounter, solutions.Length);
        }
    }
}