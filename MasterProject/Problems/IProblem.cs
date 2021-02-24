using MasterProject.Interfaces;
using MasterProject.Solutions;

namespace MasterProject.Problems
{
    public interface IProblem<out TProblem, in TSolution>
        : IDeepCloneable<TProblem>
        where TProblem : IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>
    {
        string Name { get; }
        double GetValueOf(TSolution solution);
    }
}