using System;
using System.Linq;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.NewGenerationCreators
{
    public sealed class NewGenerationCreatorChildrenPLusBest<TAlgorithm, TProblem, TSolution>
        : NewGenerationCreator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public override string SubType => $"CO{Elites}";
        private Func<TSolution>? GetBest { get; set; }

        public NewGenerationCreatorChildrenPLusBest(int newGenerationSize, int elites)
            : base(newGenerationSize, elites)
        {
            GetBest = null;
        }

        public override NewGenerationCreator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new NewGenerationCreatorChildrenPLusBest<TAlgorithm, TProblem, TSolution>(NewGenerationSize, Elites)
            {
                GetBest = (Func<TSolution>?) GetBest?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            GetBest = () => algorithm.Best;
        }

        public override TSolution[] Create(TSolution[] population, TSolution[] children)
        {
            Assert(population.Length + children.Length + 1 >= NewGenerationSize);
            Assert(GetBest != null);

            var best = GetBest();
            var sortedPopulation = population.OrderByDescending(individual => individual.Fitness)
                                             .ToArray();
            var sortedChildren = children.OrderByDescending(individual => individual.Fitness)
                                         .ToArray();

            return sortedPopulation.Take(Elites)
                                   .Prepend(best.DeepClone())
                                   .Concat(sortedChildren)
                                   .Concat(sortedPopulation)
                                   .Take(NewGenerationSize)
                                   .ToArray();
        }
    }
}