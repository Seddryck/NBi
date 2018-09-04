using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed.Result
{
    public class ScoreResult : DetailledResult
    {
        public decimal Score { get; set; }
        public decimal Threshold { get; set; }
    }
}
