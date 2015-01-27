using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossFullCaseAction : CrossAbstractCaseAction
    {
        public string SecondSet { get; set; }

        public CrossFullCaseAction(string firstSet, string secondSet)
            : base(firstSet)
        {
            SecondSet = secondSet;
        }

        protected override void ExecutePreChecks(GenerationState state)
        {
            base.ExecutePreChecks(state);

            if (!state.TestCaseSetCollection.ItemExists(SecondSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", SecondSet), "secondSet");
        }

        protected override IDataReader CrossContent(GenerationState state)
        {
            return CrossContent(
                state.TestCaseSetCollection.Item(FirstSet).Content
                , state.TestCaseSetCollection.Item(SecondSet).Content
                , delegate { return true; }
            );
        }

        public override string Display
        {
            get
            {
                return string.Format("Fully crossing sets of test cases '{0}' and '{1}'", FirstSet, SecondSet);
            }
        }

        
    }
}
