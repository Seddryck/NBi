using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    class FilterInCaseAction : FilterCaseAction
    {
        public FilterInCaseAction(string text, string column)
            : base(text, column)
        {
        }

        public override void Execute(GenerationState state)
        {
            state.TestCases.FilterIn(Column, Text);
        }

        public override string Display
        {
            get
            {
                return string.Format("Filtering in on base of '{0}' on column '{1}'", Text, Column);
            }
        }
    }
}
