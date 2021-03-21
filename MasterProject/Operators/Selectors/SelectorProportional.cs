using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Array;
using static MasterProject.Extensions.RandomExtensions;

namespace MasterProject.Operators.Selectors
{
    public sealed class SelectorProportional<TAlgorithm, TProblem, TSolution>
        : Selector<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public override string SubType => "P";
        private double[] SelectionProbabilities { get; set; }

        public SelectorProportional(int childrenCount, int parentsCount, bool isDuplicatesPossible = false)
            : base(childrenCount, parentsCount, isDuplicatesPossible)
        {
            SelectionProbabilities = Empty<double>();
        }

        public override Selector<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new SelectorProportional<TAlgorithm, TProblem, TSolution>(ChildrenCount, ParentsCount,
                                                                             IsDuplicatesPossible);
        }

        public override void BeforeSelection(TSolution[] solutions)
        {
            SelectionProbabilities = new double[solutions.Length];
            for (int i = 0; i < SelectionProbabilities.Length; i++)
            {
                SelectionProbabilities[i] = solutions[i].Fitness;
            }
        }

        protected override TSolution SelectOne(Random random, TSolution[] solutions)
        {
            return solutions[random.Next(SelectionProbabilities)];
        }
    }
}