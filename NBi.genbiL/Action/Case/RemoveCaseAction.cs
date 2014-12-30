using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class RemoveCaseAction : ICaseAction
    {
        private List<string> variableNames { get; set; }
        public IReadOnlyList<string> Variables
        {
            get
            {
                return variableNames.AsReadOnly();
            }
        }

        public RemoveCaseAction(string variableName)
        {
            variableNames = new List<string>() {variableName};
        }

        public RemoveCaseAction(IEnumerable<string> variableNames)
        {
            this.variableNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state)
        {
            foreach (var variableName in variableNames)
            {
                state.TestCaseSetCollection.Scope.Content.Columns.Remove(variableName);
            }
        }

        public string Display
        {
            get
            {
                if (variableNames.Count()==1)
                    return string.Format("Removing column '{0}'", variableNames.ElementAt(0));
                else
                    return string.Format("Removing columns '{0}'", String.Join("', '",variableNames.ToArray()));
            }
        }
    }
}
