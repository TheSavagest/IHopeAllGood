using MasterProject.Solutions;

namespace MasterProject.Extensions
{
    public static class SolutionExtensions
    {
        public static bool IsBetterThan<TSolution>(this ISolution<TSolution> it, ISolution<TSolution> other)
            where TSolution : ISolution<TSolution>
        {
            return it.Fitness > other.Fitness;
        }
    }
}