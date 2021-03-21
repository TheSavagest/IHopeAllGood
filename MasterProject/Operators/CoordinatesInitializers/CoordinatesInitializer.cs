using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;

namespace MasterProject.Operators.CoordinatesInitializers
{
    public abstract class CoordinatesInitializer<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, CoordinatesInitializer<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>, new()
    {
        public string Type => "CI";
        public abstract string SubType { get; }

        public abstract CoordinatesInitializer<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract void InitializeCoordinates(TSolution[] solutions);
    }
}