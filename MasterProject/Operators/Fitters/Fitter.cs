using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators.Fitters
{
    public abstract class Fitter<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, Fitter<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public string Type => "F";
        public abstract string SubType { get; }

        public abstract Fitter<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract void Fit(TSolution[] solutions);
    }
}