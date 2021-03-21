using static System.Diagnostics.Debug;

namespace MasterProject.Statistics
{
    public sealed class AlgorithmStatistic
    {
        public string AlgorithmName { get; }
        public double[] SuccessRates { get; }
        public double[] AvgEvaluations { get; }
        public double[] AvgTimeMs { get; }

        public AlgorithmStatistic(string algorithmName, FunctionStatistic[] functionStatistics)
        {
            Assert(algorithmName.Length > 0);
            Assert(functionStatistics.Length > 0);

            AlgorithmName = algorithmName;

            SuccessRates = new double[functionStatistics.Length];
            AvgEvaluations = new double[functionStatistics.Length];
            AvgTimeMs = new double[functionStatistics.Length];
            for (var i = 0; i < functionStatistics.Length; i++)
            {
                SuccessRates[i] = functionStatistics[i].SuccessRate;
                AvgEvaluations[i] = functionStatistics[i].AvgEvaluation;
                AvgTimeMs[i] = functionStatistics[i].AvgTimeMs;
            }
        }
    }
}