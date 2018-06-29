using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossJoinCaseAction : CrossCaseAction
    {
        public string MatchingColumn { get; set; }


        public CrossJoinCaseAction(string firstSet, string secondSet, string matchingColumn)
            : base(firstSet, secondSet)
        {
            MatchingColumn = matchingColumn;
        }

        public override bool MatchingRow(DataRow first, DataRow second) 
            => first[MatchingColumn].Equals(second[MatchingColumn]);

        public override string Display
        {
            get => $"Crossing the set of test-cases '{FirstSet}' with '{SecondSet}' on column '{MatchingColumn}'";
        }

    }
}
