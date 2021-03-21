using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators.Evaluators
{
    public abstract class Evaluator<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, Evaluator<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public string Type => "E";
        public abstract string SubType { get; }

        protected int EvaluationCounter;
        public int Evaluation => EvaluationCounter;

        protected Evaluator()
        {
            EvaluationCounter = 0;
        }

        public abstract Evaluator<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract void Evaluate(TSolution[] solutions);
    }
}