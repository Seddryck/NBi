using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Result
{
    public class AggregatedResult
    {
        public IEnumerable<DetailledResult> Details { get; }

        public int Count => Details.Count();
        public int Successes => Details.Count(r => r.IsSuccess);
        public int Failures => Details.Count(r => !r.IsSuccess);

        public AggregatedResult(IEnumerable<DetailledResult> details)
        {
            Details = details;
        }
    }
}
