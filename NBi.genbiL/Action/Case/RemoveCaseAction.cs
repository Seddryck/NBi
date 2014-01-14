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
            state.TestCases.Variables.Remove(VariableName);
            state.TestCases.Content.Columns.Remove(VariableName);
        }
    }
}
