using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;

namespace MasterProject.Operators.NewGenerationCreators
{
    public abstract class NewGenerationCreator<TAlgorithm, TProblem, TSolution>
        : IOperator<TAlgorithm, NewGenerationCreator<TAlgorithm, TProblem, TSolution>, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        public string Type => "F";
        public abstract string SubType { get; }

        protected int NewGenerationSize { get; }
        protected int Elites { get; }

        protected NewGenerationCreator(int newGenerationSize, int elites)
        {
            Assert(newGenerationSize > 0);
            Assert(elites >= 0);

            NewGenerationSize = newGenerationSize;
            Elites = elites;
        }

        public abstract NewGenerationCreator<TAlgorithm, TProblem, TSolution> DeepClone();

        public abstract void SetAlgorithm(TAlgorithm algorithm);

        public abstract TSolution[] Create(TSolution[] population, TSolution[] children);
    }
}