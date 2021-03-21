using MasterProject.Extensions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static MasterProject.Extensions.ArrayDoubleExtensions;

namespace MasterProject.Problems.Functions
{
    public abstract class Function<TSolution>
        : IProblem<Function<TSolution>, TSolution>
        where TSolution : IPoint<TSolution>
    {
        public string Name { get; }
        public int Dimension { get; }
        public TypeOfFunction TypeOfFunction { get; }
        public double[] OptimalCoordinates { get; }
        public double[] LowerSearchBorders { get; }
        public double[] UpperSearchBorders { get; }

        protected Function(string name, int dimension, TypeOfFunction typeOfFunction, double[] optimalCoordinates)
            : this(name, dimension, typeOfFunction, optimalCoordinates, NewByValue(dimension, -10.0),
                   NewByValue(dimension, 10.0))
        {
        }

        protected Function(string name, int dimension, TypeOfFunction typeOfFunction, double[] optimalCoordinates, 
                           double[] lowerSearchBorders, double[] upperSearchBorders)
        {
            Assert(name.Length > 0);
            Assert(dimension > 0);
            Assert(typeOfFunction != TypeOfFunction.Unknown);
            Assert(dimension == optimalCoordinates.Length);
            Assert(dimension == lowerSearchBorders.Length);
            Assert(dimension == upperSearchBorders.Length);

            Name = $"{name}-{dimension}D";
            Dimension = dimension;
            TypeOfFunction = typeOfFunction;
            OptimalCoordinates = optimalCoordinates.Copy();
            LowerSearchBorders = lowerSearchBorders.Copy();
            UpperSearchBorders = upperSearchBorders.Copy();
        }

        public abstract Function<TSolution> DeepClone();

        public abstract double GetValueOf(TSolution solution);
    }
}