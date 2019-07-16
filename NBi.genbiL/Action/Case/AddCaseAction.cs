using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    public class AddCaseAction : ICaseAction
    {
        
        public string VariableName { get; private set; }
        public string DefaultValue { get; private set; }
        public AddCaseAction(string variableName)
            : this(variableName, "(none)")
        {
        }

        public AddCaseAction(string variableName, string defaultValue)
        {
            VariableName = variableName;
            DefaultValue = defaultValue;
        }

        public void Execute(GenerationState state)
        {
            if (state.TestCaseCollection.Scope.Variables.Contains(VariableName))
                throw new ArgumentException(String.Format("Variable '{0}' already existing.", VariableName));

            state.TestCaseCollection.Scope.Variables.Add(VariableName);
            var newColumn = new DataColumn(VariableName);
            newColumn.DefaultValue = DefaultValue;
            state.TestCaseCollection.Scope.Content.Columns.Add(newColumn);
        }

        public string Display
        {
            get
            {
                return string.Format("Adding column '{0}'", VariableName);
            }
        }
    }
}
