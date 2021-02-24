using MasterProject.Algorithms;
using MasterProject.Interfaces;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators
{
    public interface IOperator<in TAlgorithm, out TOperator, TProblem, TSolution>
        : IDeepCloneable<TOperator>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TOperator : IOperator<TAlgorithm, TOperator, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>
    {
        public string Type { get; }
        public string SubType { get; }
        public string Name => $"{Type}-{SubType}";

        public void SetAlgorithm(TAlgorithm algorithm);
    }
}