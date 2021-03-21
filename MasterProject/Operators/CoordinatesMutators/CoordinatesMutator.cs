using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;

namespace MasterProject.Operators.CoordinatesMutators
{
    public abstract class CoordinatesMutator<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, CoordinatesMutator<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>, new()
    {
        public string Type => "CM";
        public abstract string SubType { get; }

        public abstract CoordinatesMutator<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract void MutateCoordinates(TSolution[] solutions);
    }
}