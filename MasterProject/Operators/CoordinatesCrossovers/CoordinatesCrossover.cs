using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;

namespace MasterProject.Operators.CoordinatesCrossovers
{
    public abstract class CoordinatesCrossover<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, CoordinatesCrossover<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>
    {
        public string Type => "CC";
        public abstract string SubType { get; }

        public abstract CoordinatesCrossover<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract void CrossCoordinates(TSolution[][] parents, TSolution[] children);
    }
}