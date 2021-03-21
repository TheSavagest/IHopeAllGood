using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.RandomGenerators;
using MasterProject.Solutions;
using MasterProject.Statistics;
using static System.Diagnostics.Debug;

namespace MasterProject.Testing
{
    public sealed class Tester<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        private Writer Writer { get; }
        private int NumberOfRuns { get; }
        private bool IsParallelTestingPossible { get; }
        private Random _random { get; }

        private int _progressCounter;
        private int _maxProgress;

        public Tester(Writer writer,
                      int numberOfRuns,
                      bool isParallelTestingPossible,
                      int seed)
        {
            Assert(numberOfRuns > 0);

            Writer = writer.Copy();
            NumberOfRuns = numberOfRuns;
            IsParallelTestingPossible = isParallelTestingPossible;
            if (IsParallelTestingPossible)
            {
                _random = new ThreadSafeRandom(seed);
            }
            else
            {
                _random = new Random(seed);
            }
        }

        public void TestAll(IReadOnlyList<TAlgorithm> algorithms, IReadOnlyList<TProblem> problems)
        {
            Assert(algorithms.Any());
            Assert(problems.Any());

            _progressCounter = 0;
            _maxProgress = algorithms.Count * problems.Count;

            var algorithmsStatistics = new AlgorithmStatistic[algorithms.Count];

            for (var i = 0; i < algorithms.Count; i++)
            {
                algorithmsStatistics[i] = TestOne(algorithms[i], problems);
            }

            Writer.WriteAll($";{string.Join(';', problems.Select(function => function.Name))}",
                            algorithmsStatistics);
        }

        private AlgorithmStatistic TestOne(TAlgorithm algorithm, IReadOnlyList<TProblem> problems)
        {
            Assert(problems.Any());

            var functionStatistics = new FunctionStatistic[problems.Count];

            for (var i = 0; i < functionStatistics.Length; i++)
            {
                functionStatistics[i] = TestCase(algorithm, problems[i]);
            }

            Writer.Write($"{algorithm.Name}",
                         FunctionStatistic.Header,
                         functionStatistics);

            return new AlgorithmStatistic(algorithm.Name, functionStatistics);
        }

        private FunctionStatistic TestCase(TAlgorithm algorithm, TProblem problem)
        {
            _progressCounter++;
            Console.WriteLine($"{_progressCounter}/{_maxProgress}");

            var runStatistics = new RunStatistic[NumberOfRuns];

            if (IsParallelTestingPossible && !algorithm.IsParallel)
            {
                Parallel.For(0,
                             NumberOfRuns,
                             i => runStatistics[i] = TestRun(algorithm,
                                                             problem,
                                                             i));
            }
            else
            {
                for (var i = 0; i < NumberOfRuns; i++)
                {
                    runStatistics[i] = TestRun(algorithm, problem, i);
                }
            }

            Writer.Write($"{algorithm.Name}_{problem.Name}",
                         RunStatistic.Header,
                         runStatistics);

            return new FunctionStatistic(problem.Name, runStatistics);
        }

        private RunStatistic TestRun(TAlgorithm algorithm,
                                     TProblem problem,
                                     int runIndex)
        {
            Console.WriteLine($"{algorithm.Name} {problem.Name} {runIndex}");

            var runnableAlgorithm = algorithm.DeepClone();
            runnableAlgorithm.BeforeRun(runIndex, problem, _random.Next());
            runnableAlgorithm.Run();

            var runStatistic = runnableAlgorithm.RunStatistic;
            runStatistic.RunIndex = runIndex;

            return runStatistic;
        }
    }
}