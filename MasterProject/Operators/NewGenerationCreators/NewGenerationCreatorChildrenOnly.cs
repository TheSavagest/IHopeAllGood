using System.Linq;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.NewGenerationCreators
{
    public sealed class NewGenerationCreatorChildrenOnly<TAlgorithm, TProblem, TSolution>
        : NewGenerationCreator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public override string SubType => $"CO{Elites}";

        public NewGenerationCreatorChildrenOnly(int newGenerationSize, int elites)
            : base(newGenerationSize, elites)
        {
        }

        public override NewGenerationCreator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new NewGenerationCreatorChildrenOnly<TAlgorithm, TProblem, TSolution>(NewGenerationSize, Elites);
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
        }

        public override TSolution[] Create(TSolution[] population, TSolution[] children)
        {
            Assert(population.Length + children.Length >= NewGenerationSize);

            var sortedPopulation = population.OrderByDescending(individual => individual.Fitness)
                                             .ToArray();
            var sortedChildren = children.OrderByDescending(individual => individual.Fitness)
                                         .ToArray();

            return sortedPopulation.Take(Elites)
                                   .Concat(sortedChildren)
                                   .Concat(sortedPopulation)
                                   .Take(NewGenerationSize)
                                   .ToArray();
        }
    }
}