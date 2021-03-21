using MasterProject.Algorithms;
using MasterProject.Interfaces;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators.StopConditions
{
    public abstract class StopRule<TAlgorithm, TProblem, TSolution>
        : IDeepCloneable<StopRule<TAlgorithm, TProblem, TSolution>>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public abstract void SetAlgorithm(TAlgorithm algorithm);
        public abstract bool IsStop();
        public abstract StopRule<TAlgorithm, TProblem, TSolution> DeepClone();
    }
}