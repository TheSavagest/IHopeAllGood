using System;
using MasterProject.Extensions;
using MasterProject.Operators.MainOperators;
using MasterProject.Operators.NewGenerationCreators;
using MasterProject.Operators.StopConditions;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static MasterProject.Extensions.ArrayDoubleExtensions;
using static System.Diagnostics.Debug;

namespace MasterProject.Algorithms
{
    public sealed class IterativeAlgorithm<TProblem, TSolution>
        : Algorithm<IterativeAlgorithm<TProblem, TSolution>, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : IPoint<TSolution>, new()
    {
        protected override string Type => "I";
        public override bool IsParallel => false;

        private NewGenerationCreator<IterativeAlgorithm<TProblem, TSolution>, TProblem, TSolution> NewGenerationCreator
        {
            get;
        }

        public IterativeAlgorithm(int populationSize,
                                  StopCondition<IterativeAlgorithm<TProblem, TSolution>, TProblem, TSolution>
                                      stopCondition,
                                  MainOperator<IterativeAlgorithm<TProblem, TSolution>, TProblem, TSolution>
                                      mainOperator,
                                  NewGenerationCreator<IterativeAlgorithm<TProblem, TSolution>, TProblem, TSolution>
                                      newGenerationCreator)
            : base(populationSize, stopCondition, mainOperator)
        {
            NewGenerationCreator = newGenerationCreator.DeepClone();
        }

        public override IterativeAlgorithm<TProblem, TSolution> DeepClone()
        {
            return new IterativeAlgorithm<TProblem, TSolution>(Population.Length, StopCondition, MainOperator,
                                                               NewGenerationCreator);
        }

        public override void BeforeRun(int runIndex, TProblem problem, int randomSeed)
        {
            base.BeforeRun(runIndex, problem, randomSeed);

            Random = new Random(randomSeed);
            NewGenerationCreator.SetAlgorithm(this);
        }

        protected override void UpdateBest(TSolution candidate)
        {
            Assert(Problem != null);
            
            if (candidate.IsBetterThan(Best))
            {
                Best = candidate.DeepClone();

                RunStatistic.Distance = Distance(Best.Coordinates, Problem.OptimalCoordinates);
                RunStatistic.IsSuccess = StopCondition.GetResult() == StopResult.Success;
                RunStatistic.Value = Best.Value;
                RunStatistic.Evaluation = MainOperator.Evaluation;
                RunStatistic.TimeMs = Stopwatch.ElapsedMilliseconds;
            }
        }

        public override void Run()
        {
            Stopwatch.Restart();

            MainOperator.Initialize(Population);
            UpdateBest();

            while (StopCondition.GetResult() == StopResult.NotStop)
            {
                var newGeneration = MainOperator.Apply(Population);
                Population = NewGenerationCreator.Create(Population, newGeneration);
                UpdateBest();
            }

            Stopwatch.Stop();
        }
    }
}