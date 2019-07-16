using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class RemoveCaseAction : ISingleCaseAction
    {
        private List<string> VariableNames { get; set; }
        public IReadOnlyList<string> Variables
        {
            get
            {
                return VariableNames.AsReadOnly();
            }
        }

        public RemoveCaseAction(IEnumerable<string> variableNames)
        {
            this.VariableNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state) => Execute(state.TestCaseCollection.CurrentScope);
        public void Execute(TestCases testCases)
        {
            foreach (var variableName in VariableNames)
                testCases.Content.Columns.Remove(variableName);
        }

        public string Display => $"Removing column{(VariableNames.Count() <= 1 ? "" : "s")} '{(VariableNames.Count()==1 ? VariableNames.ElementAt(0) : string.Join("', '",VariableNames.ToArray()))}'";
    }
}