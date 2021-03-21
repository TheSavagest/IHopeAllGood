using MasterProject.Algorithms;
using MasterProject.Operators.Evaluators;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators.MainOperators
{
    public abstract class MainOperator<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, MainOperator<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public string Type => "MO";
        public abstract string SubType { get; }

        protected Evaluator<TAlgorithm, TProblem, TSolution> Evaluator { get; }

        public int Evaluation => Evaluator.Evaluation;

        protected MainOperator(Evaluator<TAlgorithm, TProblem, TSolution> evaluator)
        {
            Evaluator = evaluator.DeepClone();
        }

        public abstract MainOperator<TAlgorithm, TProblem, TSolution> DeepClone();

        public virtual void SetAlgorithm(TAlgorithm algorithm)
        {
            Evaluator.SetAlgorithm(algorithm);
        }

        public abstract void Initialize(TSolution[] population);

        public abstract TSolution[] Apply(TSolution[] population);
    }
}