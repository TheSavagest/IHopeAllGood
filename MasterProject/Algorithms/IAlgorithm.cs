using System;
using MasterProject.Interfaces;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Algorithms
{
    public abstract class Algorithm<TAlgorithm, TProblem, TSolution>
        : IDeepCloneable<TAlgorithm>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>
    {
        public TProblem? Problem { get; }
        public abstract Random Random { get; }

        protected Algorithm()
        {
            Problem = null;
        }

        public abstract TAlgorithm DeepClone();
    }
}