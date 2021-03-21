using System.Collections.Generic;
using System.Linq;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.String;

namespace MasterProject.Operators.CoordinatesMutators
{
    public sealed class CoordinatesMutatorCombined<TAlgorithm, TProblem, TSolution>
        : CoordinatesMutator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => $"[{Join('+', Mutators.Select(m => m.SubType))}]";
        private IEnumerable<CoordinatesMutator<TAlgorithm, TProblem, TSolution>> Mutators { get; }

        public CoordinatesMutatorCombined(IEnumerable<CoordinatesMutator<TAlgorithm, TProblem, TSolution>> mutators)
        {
            Mutators = mutators.Select(m => m.DeepClone()).ToArray();
        }

        public override CoordinatesMutator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new CoordinatesMutatorCombined<TAlgorithm, TProblem, TSolution>(Mutators);
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            foreach (var mutator in Mutators)
            {
                mutator.SetAlgorithm(algorithm);
            }
        }

        public override void MutateCoordinates(TSolution[] solutions)
        {
            foreach (var mutator in Mutators)
            {
                mutator.MutateCoordinates(solutions);
            }
        }
    }
}