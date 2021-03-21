using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Array;
using static MasterProject.Extensions.ArrayDoubleExtensions;
using static MasterProject.Extensions.RandomExtensions;

namespace MasterProject.Operators.Selectors
{
    public sealed class SelectorLinearRank<TAlgorithm, TProblem, TSolution>
        : Selector<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public override string SubType => "LR";
        private double[] SelectionProbabilities { get; set; }

        public SelectorLinearRank(int childrenCount, int parentsCount, bool isDuplicatesPossible = false)
            : base(childrenCount, parentsCount, isDuplicatesPossible)
        {
            SelectionProbabilities = Empty<double>();
        }

        public override Selector<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new SelectorLinearRank<TAlgorithm, TProblem, TSolution>(ChildrenCount, ParentsCount,
                                                                           IsDuplicatesPossible);
        }

        public override void BeforeSelection(TSolution[] solutions)
        {
            var tmpProbabilities = new double[solutions.Length];
            for (var i = 0; i < tmpProbabilities.Length; i++)
            {
                tmpProbabilities[i] = solutions[i].Fitness;
            }

            SelectionProbabilities = Ranks(tmpProbabilities);
        }

        protected override TSolution SelectOne(Random random, TSolution[] solutions)
        {
            return solutions[random.Next(SelectionProbabilities)];
        }
    }
}