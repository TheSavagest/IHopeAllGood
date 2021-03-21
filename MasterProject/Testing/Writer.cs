using System.Collections.Generic;
using System.IO;
using System.Linq;
using MasterProject.Statistics;
using static System.Diagnostics.Debug;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Math;
using static System.String;

namespace MasterProject.Testing
{
    public sealed class Writer
    {
        private string StatisticPath { get; }

        public Writer(string statisticPath)
        {
            Assert(statisticPath.Length > 0);

            StatisticPath = statisticPath;

            CreateDirectory(StatisticPath);
        }

        public Writer Copy()
        {
            return new(StatisticPath);
        }

        public void Write(string fileName,
                          string header,
                          IEnumerable<object> statistics)
        {
            var filePath = Combine(StatisticPath, $@"{fileName}.csv");
            using var outputFile = new StreamWriter(filePath);
            outputFile.WriteLine(header);
            foreach (var statistic in statistics) outputFile.WriteLine(statistic);
        }

        public void WriteAll(string header, ICollection<AlgorithmStatistic> statistics)
        {
            //SuccessRates
            var filePathSr = Combine(StatisticPath, @"!ALL_SR.csv");
            using var outputFileSr = new StreamWriter(filePathSr);
            outputFileSr.WriteLine(header);
            foreach (var statistic in statistics)
                outputFileSr.WriteLine($"{statistic.AlgorithmName};{Join(';', statistic.SuccessRates.Select(v => $"{Round(100.0 * v)}"))}");

            //AverageEvaluations
            var filePathAe = Combine(StatisticPath, @"!ALL_AE.csv");
            using var outputFileAe = new StreamWriter(filePathAe);
            outputFileAe.WriteLine(header);
            foreach (var statistic in statistics)
                outputFileAe.WriteLine($"{statistic.AlgorithmName};{Join(';', statistic.AvgEvaluations.Select(v => Round(v)))}");

            //AverageTimeMs
            var filePathAt = Combine(StatisticPath, @"!ALL_AT.csv");
            using var outputFileAt = new StreamWriter(filePathAt);
            outputFileAt.WriteLine(header);
            foreach (var statistic in statistics)
                outputFileAt.WriteLine($"{statistic.AlgorithmName};{Join(';', statistic.AvgTimeMs.Select(v => Round(v)))}");
        }
    }
}