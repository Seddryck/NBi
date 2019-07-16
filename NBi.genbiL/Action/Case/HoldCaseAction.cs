using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class HoldCaseAction : ICaseAction
    {
        private List<string> variableNames { get; set; }

        public IReadOnlyList<string> Variables
        {
            get
            {
                return variableNames.AsReadOnly();
            }
        }
        public HoldCaseAction(string variableName)
        {
            variableNames = new List<string>() { variableName };
        }

        public HoldCaseAction(IEnumerable<string> variableNames)
        {
            this.variableNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state)
        {
            var variablesDeleted = state.TestCaseCollection.Scope.Variables.Except(variableNames).ToList();

            foreach (var variable in variablesDeleted)
            {
                state.TestCaseCollection.Scope.Variables.Remove(variable);
                state.TestCaseCollection.Scope.Content.Columns.Remove(variable);
            }
        }

        public string Display
        {
            get
            {
                if (variableNames.Count == 1)
                    return string.Format("Holding column '{0}'", variableNames[0]);
                else
                    return string.Format("Holding columns '{0}'", String.Join("', '", variableNames));
            }
        }
    }
}
