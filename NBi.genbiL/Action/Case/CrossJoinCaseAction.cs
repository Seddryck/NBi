using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

class CrossJoinCaseAction : CrossCaseAction
{
    public IEnumerable<string> MatchingColumns { get; set; }


    public CrossJoinCaseAction(string firstSet, string secondSet, IEnumerable<string> matchingColumns)
        : base(firstSet, secondSet)
    {
        MatchingColumns = matchingColumns;
    }

    public override bool MatchingRow(DataRow first, DataRow second)
    {
        var result = true;
        var enumerator = MatchingColumns.GetEnumerator();
        while (enumerator.MoveNext() && result)
            result = first[enumerator.Current].Equals(second[enumerator.Current]);
        return result;
    }

    public override string Display 
        => $"Crossing the set of test-cases '{FirstSet}' with '{SecondSet}' on column '{MatchingColumns}'";
}
