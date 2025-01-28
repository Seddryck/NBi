using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

class CrossFullCaseAction : CrossCaseAction
{
    public CrossFullCaseAction(string firstSet, string secondSet)
        : base(firstSet, secondSet){ }

    
    public override bool MatchingRow(DataRow first, DataRow second) => true;

    public override string Display => $"Fully crossing the set of test-cases '{FirstSet}' with '{SecondSet}'";
}
