using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Result
{
    public class FlatResultBuilder
    {
        public AggregatedResult Execute(ITestResult nunitResult)
        {
            var details = ParseChild(Enumerable.Repeat(nunitResult, 1));
            var aggregated = new AggregatedResult(details);
            return aggregated;
        }

        private IEnumerable<DetailledResult> ParseChild(IEnumerable<ITestResult> nunitResults)
        {
            var childResults = new List<DetailledResult>();
            foreach (var r in nunitResults)
            {
                if (r.Test.IsSuite)
                {
                    var results = ParseChild(r.Children);
                    childResults.AddRange(results);
                }
                else
                {
                    childResults.Add(ParseElement(r));
                }
            }
            return childResults;
        }

        private DetailledResult ParseElement(ITestResult nunitResult)
        {
            return new DetailledResult()
            {
                IsSuccess = nunitResult.ResultState.Status == TestStatus.Passed,
                Message = nunitResult.Message
            };
        }
    }
}
