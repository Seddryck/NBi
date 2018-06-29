using System;
using System.Collections.Generic;
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

        public override void Execute(GenerationState state)
        {
                state.TestCaseCollection.Cross(FirstSet, SecondSet, MatchingColumn);
        }

        public override string Display
        {
            get => $"Crossing test cases set '{FirstSet}' with '{SecondSet}' on column '{MatchingColumn}'";
        }

        
    }
}
