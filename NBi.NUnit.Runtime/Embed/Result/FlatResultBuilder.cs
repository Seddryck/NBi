using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Result
{
    public class FlatResultBuilder
    {
        public AggregatedResult Execute(TestResult nunitResult)
        {
            var details = ParseChild(Enumerable.Repeat(nunitResult, 1));
            var aggregated = new AggregatedResult(details);
            return aggregated;
        }

        private IEnumerable<DetailledResult> ParseChild(IEnumerable<TestResult> nunitResults)
        {
            var childResults = new List<DetailledResult>();
            foreach (var r in nunitResults)
            {
                if (r.Test.IsSuite)
                {
                    var results = ParseChild(r.Results.Cast<TestResult>());
                    childResults.AddRange(results);
                }
                else
                    childResults.Add(ParseElement(r));
            }
            return childResults;
        }

        private DetailledResult ParseElement(TestResult nunitResult)
        {
            return new DetailledResult()
            {
                IsSuccess = nunitResult.IsSuccess,
                Message = nunitResult.Message
            };
        }
    }
}
