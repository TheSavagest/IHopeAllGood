using System.Collections.Generic;
using System.Linq;
using MasterProject.Algorithms;
using MasterProject.Interfaces;
using MasterProject.Problems;
using MasterProject.Solutions;

namespace MasterProject.Operators.StopConditions
{
    public sealed class StopCondition<TAlgorithm, TProblem, TSolution>
        : IDeepCloneable<StopCondition<TAlgorithm, TProblem, TSolution>>
        where TAlgorithm : Algorithm<TAlgorithm, TProblem, TSolution>
        where TProblem : class, IProblem<TProblem, TSolution>
        where TSolution : ISolution<TSolution>, new()
    {
        private IEnumerable<StopRule<TAlgorithm, TProblem, TSolution>> FailRules { get; }
        private IEnumerable<StopRule<TAlgorithm, TProblem, TSolution>> SuccessRules { get; }

        public StopCondition(IEnumerable<StopRule<TAlgorithm, TProblem, TSolution>> failRules,
                             IEnumerable<StopRule<TAlgorithm, TProblem, TSolution>> successRules)
        {
            FailRules = failRules.Select(rule => rule.DeepClone()).ToArray();
            SuccessRules = successRules.Select(rule => rule.DeepClone()).ToArray();
        }

        public StopCondition<TAlgorithm, TProblem, TSolution> DeepClone()
        {
            return new(FailRules, SuccessRules);
        }

        public void SetAlgorithm(TAlgorithm algorithm)
        {
            foreach (var failRule in FailRules)
            {
                failRule.SetAlgorithm(algorithm);
            }

            foreach (var successRule in SuccessRules)
            {
                successRule.SetAlgorithm(algorithm);
            }
        }

        public StopResult GetResult()
        {
            var isFail = FailRules.Aggregate(false, (current, failRule) => current | failRule.IsStop());
            var isSuccess = SuccessRules.Aggregate(false, (current, successRule) => current | successRule.IsStop());

            return (isFail, isSuccess) switch
            {
                (false, false) => StopResult.NotStop,
                (false, true) => StopResult.Success,
                (true, false) => StopResult.Fail,
                (true, true) => StopResult.Unknown
            };
        }
    }
}