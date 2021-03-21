using System;
using MasterProject.Algorithms;
using MasterProject.Problems;
using MasterProject.Problems.Functions;
using MasterProject.Solutions;
using static System.Diagnostics.Debug;
using static MasterProject.Problems.Functions.TypeOfFunction;

namespace MasterProject.Operators.Fitters
{
    public sealed class FitterNormalizingMinimization<TAlgorithm, TProblem, TSolution>
        : Fitter<TAlgorithm, TProblem, TSolution>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : Function<TSolution>, IProblem<TProblem, TSolution>
        where TSolution : class, IPoint<TSolution>, new()
    {
        public override string SubType => "NM";
        private Func<TypeOfFunction>? GetTypeOfFunction { get; set; }

        public FitterNormalizingMinimization()
        {
            GetTypeOfFunction = null;
        }
        
        public override Fitter<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new FitterNormalizingMinimization<TAlgorithm, TProblem, TSolution>
            {
                GetTypeOfFunction = (Func<TypeOfFunction>?) GetTypeOfFunction?.Clone()
            };
        }

        public override void SetAlgorithm(TAlgorithm algorithm)
        {
            Assert(algorithm.Problem != null);

            GetTypeOfFunction = () => algorithm.Problem.TypeOfFunction;
        }

        public override void Fit(TSolution[] solutions)
        {
            Assert(GetTypeOfFunction != null);

            var typeOfFunction = GetTypeOfFunction(); 
            
            foreach (var solution in solutions)
            {
                solution.Fitness = typeOfFunction switch
                {
                    Minimization => 1.0 / (1.0 + solution.Value),
                    Maximization => solution.Value,
                    Unknown => throw new Exception(),
                    _ => throw new Exception()
                };
            }
        }
    }
}