using MasterProject.Algorithms;
using MasterProject.Operators;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Extensions
{
    public static class OperatorExtensions
    {
        public static string GetName<TAlgorithm, TOperator, TProblem, TSolution>(
            this IOperator<TAlgorithm, TOperator, TProblem, TSolution> it)
            where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
            where TOperator : IOperator<TAlgorithm, TOperator, TProblem, TSolution>
            where TProblem : class, IProblem<TProblem, TSolution>
            where TSolution : ISolution<TSolution>, new()
        {
            return $"{it.Type}-{it.SubType}";
        }
    }
}