using System;
using System.Diagnostics;
using System.Linq;
using MasterProject.Extensions;
using MasterProject.Interfaces;
using MasterProject.Operators.MainOperators;
using MasterProject.Operators.StopConditions;
using MasterProject.Problems;
using MasterProject.Solutions;
using MasterProject.Statistics;
using static System.Diagnostics.Debug;

namespace MasterProject.Algorithms
{
    public abstract class Algorithm<TAlgorithm, TProblem, TSolution>
        : IDeepCloneable<TAlgorithm>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        protected abstract string Type { get; }
        public string Name => $"{Type}_{MainOperator.GetName()}"; 
        public abstract bool IsParallel { get; }

        protected Stopwatch Stopwatch { get; }
        public RunStatistic RunStatistic { get; }

        public TProblem? Problem { get; private set; }

        protected TSolution[] Population { get; set; }
        public TSolution Best { get; protected set; }

        public Random? Random { get; protected set; }

        protected StopCondition<TAlgorithm, TProblem, TSolution> StopCondition { get; }
        protected MainOperator<TAlgorithm, TProblem, TSolution> MainOperator { get; }

        public int Evaluations => MainOperator.Evaluation;

        protected Algorithm(int populationSize,
                            StopCondition<TAlgorithm, TProblem, TSolution> stopCondition,
                            MainOperator<TAlgorithm, TProblem, TSolution> mainOperator)
        {
            Assert(populationSize > 0);

            Stopwatch = new Stopwatch();
            RunStatistic = new RunStatistic();

            Problem = null;

            Population = new TSolution[populationSize];
            for (int i = 0; i < Population.Length; i++)
            {
                Population[i] = new TSolution();
            }
            Best = new TSolution
            {
                Fitness = double.MinValue
            };

            Random = null;
            
            StopCondition = stopCondition.DeepClone();
            MainOperator = mainOperator.DeepClone();
        }

        public abstract TAlgorithm DeepClone();

        public virtual void BeforeRun(int runIndex, TProblem problem, int randomSeed)
        {
            RunStatistic.RunIndex = runIndex;
            Problem = problem.DeepClone();
            StopCondition.SetAlgorithm((TAlgorithm) this);
            MainOperator.SetAlgorithm((TAlgorithm) this);
        }

        protected void UpdateBest()
        {
            var bestCandidate = Population.OrderBy(ind => ind.Fitness).First();
            UpdateBest(bestCandidate);
        }

        protected abstract void UpdateBest(TSolution candidate);

        public abstract void Run();
    }
}