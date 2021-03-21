using MasterProject.Algorithms;
using MasterProject.Extensions;
using MasterProject.Operators.CoordinatesCrossovers;
using MasterProject.Operators.CoordinatesInitializers;
using MasterProject.Operators.CoordinatesMutators;
using MasterProject.Operators.Evaluators;
using MasterProject.Operators.Fitters;
using MasterProject.Operators.Selectors;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;

namespace MasterProject.Operators.MainOperators
{
    public class MainOperatorRCGA<TAlgorithm, TProblem, TSolution>
        : MainOperator<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>, new()
    {
        public override string SubType => $"RCGA[{Selector.GetName()}+{Crossover.GetName()}+{Mutator.GetName()}]";

        private CoordinatesInitializer<TAlgorithm, TProblem, TSolution> Initializer { get; }
        private Fitter<TAlgorithm, TProblem, TSolution> Fitter { get; }
        private Selector<TAlgorithm, TProblem, TSolution> Selector { get; }
        private CoordinatesCrossover<TAlgorithm, TProblem, TSolution> Crossover { get; }
        private CoordinatesMutator<TAlgorithm, TProblem, TSolution> Mutator { get; }
        
        public MainOperatorRCGA(Evaluator<TAlgorithm, TProblem, TSolution> evaluator, 
                                CoordinatesInitializer<TAlgorithm, TProblem, TSolution> initializer, 
                                Fitter<TAlgorithm, TProblem, TSolution> fitter, 
                                Selector<TAlgorithm, TProblem, TSolution> selector, 
                                CoordinatesCrossover<TAlgorithm, TProblem, TSolution> crossover, 
                                CoordinatesMutator<TAlgorithm, TProblem, TSolution> mutator)
            : base(evaluator)
        {
            Initializer = initializer.DeepClone();
            Fitter = fitter.DeepClone();
            Selector = selector.DeepClone();
            Crossover = crossover.DeepClone();
            Mutator = mutator.DeepClone();
        }

        public override MainOperator<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new MainOperatorRCGA<TAlgorithm, TProblem, TSolution>(Evaluator, Initializer, Fitter, Selector, 
                                                                         Crossover, Mutator);
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            base.SetAlgorithm(algorithm);
            
            Initializer.SetAlgorithm(algorithm);
            Fitter.SetAlgorithm(algorithm);
            Selector.SetAlgorithm(algorithm);
            Crossover.SetAlgorithm(algorithm);
            Mutator.SetAlgorithm(algorithm);
        }

        public override void Initialize(TSolution[] population)
        {
            Initializer.InitializeCoordinates(population);
            Evaluator.Evaluate(population);
            Fitter.Fit(population);
        }

        public override TSolution[] Apply(TSolution[] population)
        {
            Selector.BeforeSelection(population);
            var parents = Selector.Select(population);
            
            var children = new TSolution[parents.Length];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = new TSolution();
            }
            Crossover.CrossCoordinates(parents, children);
            
            Mutator.MutateCoordinates(children);
            Evaluator.Evaluate(children);
            Fitter.Fit(children);
            return children;
        }
    }
}