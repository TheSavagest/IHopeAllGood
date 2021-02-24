using MasterProject.Solutions;

namespace MasterProject.Problems.Functions
{
    public abstract class Function<TSolution>
        : IProblem<Function<TSolution>, TSolution>
        where TSolution : IPoint<TSolution>
    {
        public abstract string Name { get; }
        public int Dimension { get; }
        private TypeOfFunction TypeOfFunction { get; }
        private double[] OptimalCoordinates { get; }
        public double[] LowerSearchBorders { get; }
        public double[] UpperSearchBorders { get; }

        protected Function(int dimension, TypeOfFunction typeOfFunction, double[] optimalCoordinates,
                           double[] lowerSearchBorders, double[] upperSearchBorders)
        {
            Dimension = dimension;
            TypeOfFunction = typeOfFunction;
            OptimalCoordinates = optimalCoordinates;
            LowerSearchBorders = lowerSearchBorders;
            UpperSearchBorders = upperSearchBorders;
        }

        public abstract Function<TSolution> DeepClone();

        public abstract double GetValueOf(TSolution solution);
    }
}