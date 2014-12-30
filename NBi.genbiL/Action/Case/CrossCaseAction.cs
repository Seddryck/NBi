using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossCaseAction : ICaseAction
    {
        public string FirstSet { get; set; }
        public string SecondSet { get; set; }
        public string MatchingColumn { get; set; }
        public bool IsMatchingColumn { get; set; }

        public CrossCaseAction(string firstSet, string secondSet)
        {
            FirstSet = firstSet;
            SecondSet = secondSet;
            IsMatchingColumn = false;
        }

        public CrossCaseAction(string firstSet, string secondSet, string matchingColumn)
            : this(firstSet, secondSet)
        {
            MatchingColumn = matchingColumn;
            IsMatchingColumn = true;
        }

        public void Execute(GenerationState state)
        {
            if (IsMatchingColumn)
                state.TestCaseCollection.Cross(FirstSet, SecondSet, MatchingColumn);
            else
                state.TestCaseCollection.Cross(FirstSet, SecondSet);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Crossing test cases set '{0}' with '{1}'", FirstSet, SecondSet);
            }
        }

        
    }
}
