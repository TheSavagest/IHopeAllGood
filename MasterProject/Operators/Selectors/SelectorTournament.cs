using System;
using MasterProject.Algorithms;
using MasterProject.Extensions;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static MasterProject.Extensions.ArrayIntExtensions;

namespace MasterProject.Operators.Selectors
{
    public sealed class SelectorTournament<TAlgorithm, TProblem, TSolution>
        : Selector<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public override string SubType => "P";
        private int TournamentSize { get; }

        public SelectorTournament(int childrenCount, int parentsCount, bool isDuplicatesPossible = false, 
                                  int tournamentSize = 4)
            : base(childrenCount, parentsCount, isDuplicatesPossible)
        {
            TournamentSize = tournamentSize;
        }

        public override Selector<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new SelectorTournament<TAlgorithm, TProblem, TSolution>(ChildrenCount, ParentsCount,
                                                                           IsDuplicatesPossible, TournamentSize);
        }

        public override void BeforeSelection(TSolution[] solutions)
        {
        }

        protected override TSolution SelectOne(Random random, TSolution[] solutions)
        {
            Assert(solutions.Length > TournamentSize);

            int[] tournamentIndexes = Unique(TournamentSize, 0, solutions.Length, random);
            var bestCandidate = solutions[tournamentIndexes[0]];

            for (int t = 1; t < tournamentIndexes.Length; t++)
            {
                if (solutions[tournamentIndexes[t]].IsBetterThan(bestCandidate))
                {
                    bestCandidate = solutions[tournamentIndexes[t]];
                }
            }

            return bestCandidate;
        }
    }
}