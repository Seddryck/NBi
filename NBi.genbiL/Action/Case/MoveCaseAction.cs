using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class MoveCaseAction : ICaseAction
    {
        public string VariableName { get; set; }

        /// <summary>
        /// Int.Min = First
        /// -1 = left
        /// +1 = right
        /// Int.Max = Last
        /// </summary>
        public int Ordinal { get; set; }
        public MoveCaseAction(string variableName, int ordinal)
        {
            VariableName = variableName;
            Ordinal = ordinal;
        }


        public void Execute(GenerationState state)
        {
            var currentPosition = state.TestCaseCollection.Scope.Variables.IndexOf(VariableName);

            if (Ordinal != int.MinValue && Ordinal != int.MaxValue)
                state.TestCaseCollection.Scope.MoveVariable(VariableName, currentPosition + Ordinal);

            if (Ordinal == int.MinValue)
                state.TestCaseCollection.Scope.MoveVariable(VariableName, 0);

            if (Ordinal == int.MaxValue)
                state.TestCaseCollection.Scope.MoveVariable(VariableName, state.TestCaseCollection.Scope.Variables.Count-1);
        }

        public string Display
        {
            get
            {
                if (Ordinal != int.MinValue && Ordinal != int.MaxValue)
                    return string.Format("Moving column '{0}' to the {1}", VariableName, Ordinal == 1 ? "right" : "left");
                else
                    return string.Format("Moving column '{0}' to the extreme {1}", VariableName, Ordinal > 1 ? "right" : "left");
            }
        }
    }
}
