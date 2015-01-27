using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossColumnMatchingCaseAction : CrossFullCaseAction
    {
        public string MatchingColumn { get; set; }

        
        public CrossColumnMatchingCaseAction(string firstSet, string secondSet, string matchingColumn)
            : base(firstSet, secondSet)
        {
            MatchingColumn = matchingColumn;
        }

        protected override void ExecutePreChecks(GenerationState state)
        {
            base.ExecutePreChecks(state);

            if (!state.TestCaseSetCollection.Item(FirstSet).Content.Columns.Contains(MatchingColumn))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't contain a column named '{1}'.", FirstSet, MatchingColumn));

            if (!state.TestCaseSetCollection.Item(SecondSet).Content.Columns.Contains(MatchingColumn))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't contain a column named '{1}'.", SecondSet, MatchingColumn));
        }

        protected override IDataReader CrossContent(GenerationState state)
        {
            Func<DataRow, DataRow, bool> matchingRow = (a, b) => a[MatchingColumn].Equals(b[MatchingColumn]);
            
            return CrossContent(
                state.TestCaseSetCollection.Item(FirstSet).Content
                , state.TestCaseSetCollection.Item(SecondSet).Content
                , matchingRow
            );
        }


        public override string Display
        {
            get
            {
                return string.Format("Crossing the sets of test cases '{0}' and '{1}' when matching on column '{2}'", FirstSet, SecondSet, MatchingColumn);
            }
        }

        
    }
}
