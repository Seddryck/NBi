using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class RemoveCaseAction : ICaseAction
    {
        public string VariableName { get; set; }
        public RemoveCaseAction(string variableName)
        {
            VariableName = variableName;
        }

        public void Execute(GenerationState state)
        {
            state.TestCaseCollection.Focus.Variables.Remove(VariableName);
            state.TestCaseCollection.Focus.Content.Columns.Remove(VariableName);
        }

        public string Display
        {
            get
            {
                return string.Format("Removing column '{0}'", VariableName);
            }
        }
    }
}
