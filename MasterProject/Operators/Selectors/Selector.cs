using System;
using System.Linq;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.Selectors
{
    public abstract class Selector<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, Selector<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public string Type => "S";
        public abstract string SubType { get; }

        protected int ChildrenCount { get; }
        protected int ParentsCount { get; }
        protected bool IsDuplicatesPossible { get; }
        private Func<Random>? GetRandom { get; set; }

        protected Selector(int childrenCount, int parentsCount, bool isDuplicatesPossible)
        {
            Assert(childrenCount > 0);
            Assert(parentsCount > 0);

            ChildrenCount = childrenCount;
            ParentsCount = parentsCount;
            IsDuplicatesPossible = isDuplicatesPossible;
            GetRandom = null;
        }

        public abstract Selector<TAlgorithm, TProblem, TSolution> DeepClone();

        public void SetAlgorithm(TAlgorithm algorithm)
        {
            GetRandom = () => algorithm.Random;
        }

        public abstract void BeforeSelection(TSolution[] solutions);

        protected abstract TSolution SelectOne(Random random, TSolution[] solutions);

        public TSolution[][] Select(TSolution[] solutions)
        {
            Assert(solutions.Length > ParentsCount);
            Assert(GetRandom != null);

            var parents = new TSolution[ChildrenCount][];
            var random = GetRandom();

            if (IsDuplicatesPossible)
            {
                for (int c = 0; c < parents.Length; c++)
                {
                    parents[c] = new TSolution[ParentsCount];

                    for (int p = 0; p < parents[c].Length; p++)
                    {
                        parents[c][p] = SelectOne(random, solutions);
                    }
                }
            }
            else
            {
                for (int c = 0; c < parents.Length; c++)
                {
                    parents[c] = new TSolution[ParentsCount];

                    for (int p = 0; p < parents[c].Length; p++)
                    {
                        var candidate = SelectOne(random, solutions);

                        while (parents[c].Contains(candidate))
                        {
                            candidate = SelectOne(random, solutions);
                        }

                        parents[c][p] = candidate;
                    }
                }
            }

            return parents;
        }
    }
}