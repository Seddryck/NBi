using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class MoveCaseAction : ICaseAction
    {
        public string VariableName { get; set; }
        public int RelativePosition { get; set; }
        public MoveCaseAction(string variableName, int relativePosition)
        {
            VariableName = variableName;
            RelativePosition = relativePosition;
        }

        public void Execute(GenerationState state)
        {
            var currentPosition = state.TestCases.Variables.IndexOf(VariableName);

            state.TestCases.MoveVariable(VariableName, currentPosition + RelativePosition);
        }

        public string Display
        {
            get
            {
                return string.Format("Moving column '{0}' to the {1}", VariableName, RelativePosition==1 ? "right": "left");
            }
        }
    }
}
