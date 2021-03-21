using System.Linq;
using static System.Diagnostics.Debug;

namespace MasterProject.Statistics
{
    public sealed class FunctionStatistic
    {
        public const string Header = "FunctionName;SuccessRate;MinEvaluation;AvgEvaluation;MaxEvaluation;MinTimeMs;AvgTimeMs;MaxTimeMs";

        private string FunctionName { get; }
        public double SuccessRate { get; }
        private int MinEvaluation { get; }
        public double AvgEvaluation { get; }
        private int MaxEvaluation { get; }
        private long MinTimeMs { get; }
        public double AvgTimeMs { get; }
        private long MaxTimeMs { get; }

        public FunctionStatistic(string functionName, RunStatistic[] runStatistics)
        {
            Assert(functionName.Length > 0);
            Assert(runStatistics.Length > 0);

            FunctionName = functionName;

            var successRuns = runStatistics.Where(statistic => statistic.IsSuccess).ToArray();

            if (successRuns.Length == 0)
            {
                SuccessRate = 0.0;

                MinEvaluation = -1;
                AvgEvaluation = -1;
                MaxEvaluation = -1;

                MinTimeMs = -1;
                AvgTimeMs = -1;
                MaxTimeMs = -1;
            }
            else
            {
                SuccessRate = successRuns.Length / (double) runStatistics.Length;

                MinEvaluation = int.MaxValue;
                AvgEvaluation = 0.0;
                MaxEvaluation = int.MinValue;

                MinTimeMs = long.MaxValue;
                AvgTimeMs = 0.0;
                MaxTimeMs = long.MinValue;

                for (var i = 0; i < successRuns.Length; i++)
                {
                    if (successRuns[i].Evaluation < MinEvaluation)
                    {
                        MinEvaluation = successRuns[i].Evaluation;
                    }
                    AvgEvaluation += successRuns[i].Evaluation;
                    if (MaxEvaluation < successRuns[i].Evaluation)
                    {
                        MaxEvaluation = successRuns[i].Evaluation;
                    }

                    if (successRuns[i].TimeMs < MinTimeMs)
                    {
                        MinTimeMs = successRuns[i].TimeMs;
                    }
                    AvgTimeMs += successRuns[i].TimeMs;
                    if (MaxTimeMs < successRuns[i].TimeMs)
                    {
                        MaxTimeMs = successRuns[i].TimeMs;
                    }
                }
                
                AvgEvaluation /= successRuns.Length;
                AvgTimeMs /= successRuns.Length;
            }
        }

        public override string ToString()
        {
            return $"{FunctionName};{SuccessRate};{MinEvaluation};{AvgEvaluation};{MaxEvaluation};{MinTimeMs};{AvgTimeMs};{MaxTimeMs}";
        }
    }
}