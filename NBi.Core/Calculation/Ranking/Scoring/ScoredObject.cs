using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking.Scoring;


public class ScoredObject
{
    public ScoredObject(object score, object value)
    {
        Score = score;
        Value = value;
    }
    public object Score { get; set; }
    public object Value { get; set; }
}
