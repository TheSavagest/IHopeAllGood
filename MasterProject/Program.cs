using System;
using System.Drawing;
using MasterProject.Algorithms;
using MasterProject.Operators.CoordinatesCrossovers;
using MasterProject.Operators.CoordinatesInitializers;
using MasterProject.Operators.CoordinatesMutators;
using MasterProject.Operators.Evaluators;
using MasterProject.Operators.Fitters;
using MasterProject.Operators.MainOperators;
using MasterProject.Operators.NewGenerationCreators;
using MasterProject.Operators.Selectors;
using MasterProject.Operators.StopConditions;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using MasterProject.Testing;
using static System.Console;
using static System.IO.Path;
using Point = MasterProject.Solutions.Point;

namespace MasterProject
{
    public static class Program
    {
        private const string StatisticFolder = @"C:\Stat";
        private const int RunNumber = 31;

        private const bool IsParallelTestingPossible = true;
        // private const bool IsParallelTestingPossible = false;

        private const double Accuracy = 0.01;
        private const int PopulationSize = 100;

        private static void Main()
        {
            Test(5);
        }

        private static void Test(int dim)
        {
            WriteLine($"---------{dim}---------");
            string statisticPath = Combine(StatisticFolder, $"{DateTime.Now:dd_MM_yyy_HH_mm}_{dim}D");
            var writer = new Writer(statisticPath);

            var tester = new Tester<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(writer, 
                                                                                                        RunNumber,
                                                                                                        IsParallelTestingPossible,
                                                                                                        13031997);
            var dimension = dim;
            var algorithms = CreateIterableRCGA(dim);
            var functions = CreateFunctions(dimension);

            tester.TestAll(algorithms, functions);

            WriteLine($"done for {dim}D, i hope all good =)");
        }

        private static IterativeAlgorithm<Function<Point>, Point>[] CreateIterableRCGA(int dimension)
        {
            var maxIteration = dimension * PopulationSize;
            var maxEvaluation = PopulationSize * maxIteration;

            var evaluator = new EvaluatorStandard<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>();
            var coordinatesInitializer =
                new CoordinatesInitializerUniform<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>();
            var fitter =
                new FitterNormalizingMinimization<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>();

            var selectors = new Selector<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
            {
                new SelectorProportional<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 2),
                new SelectorLinearRank<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 2),
                new SelectorTournament<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 2)
            };
            var crossovers = new CoordinatesCrossover<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
            {
                new CoordinatesCrossoverArithmetic<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                new CoordinatesCrossoverBLX<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                new CoordinatesCrossoverExtendedLine<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                new CoordinatesCrossoverHeuristic<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                new CoordinatesCrossoverLinear<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(true),
                new CoordinatesCrossoverLinear<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(false),
                new CoordinatesCrossoverProportional<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>()
            };
            var mutators = new CoordinatesMutator<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
            {
                new CoordinatesMutatorCombined<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(new CoordinatesMutator<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
                {
                    new CoordinatesMutatorUniform<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                    new CoordinatesMutatorClamp<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>()
                }),
                new CoordinatesMutatorCombined<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(new CoordinatesMutator<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
                {
                    new CoordinatesMutatorMPT<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(),
                    new CoordinatesMutatorClamp<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>()
                }),
            };
            var newGenerationCreators = new NewGenerationCreator<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>[]
            {
                new NewGenerationCreatorChildrenOnly<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 0),
                new NewGenerationCreatorChildrenOnly<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 5),
                new NewGenerationCreatorChildrenPLusBest<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 0),
                new NewGenerationCreatorChildrenPLusBest<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>
                    (PopulationSize, 5)
            };

            var stopCondition = new StopCondition<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(new []{new MoreEvaluationsThan<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(maxEvaluation)}, 
                                                                                                                      new []{new DistanceFromBestToOptimumIsLessThan<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(Accuracy)});

            var algorithms = new IterativeAlgorithm<Function<Point>, Point>[selectors.Length * crossovers.Length * 
                                                                            mutators.Length * 
                                                                            newGenerationCreators.Length];
            var ai = 0;

            foreach (var selector in selectors)
            {
                foreach (var crossover in crossovers)
                {
                    foreach (var mutator in mutators)
                    {
                        foreach (var newGenerationCreator in newGenerationCreators)
                        {
                            algorithms[ai] = new IterativeAlgorithm<Function<Point>, Point>(PopulationSize, 
                                                                                            stopCondition, 
                                                                                            new MainOperatorRCGA<IterativeAlgorithm<Function<Point>, Point>, Function<Point>, Point>(evaluator, coordinatesInitializer, fitter, selector, crossover, mutator), 
                                                                                            newGenerationCreator);
                            ai++;
                        }
                    }
                }
            }

            return algorithms;
        }

        private static Function<Point>[] CreateFunctions(int dimension)
        {
            return new Function<Point>[]
            {
                new Ackley<Point>(dimension)
            };
        }
    }
}