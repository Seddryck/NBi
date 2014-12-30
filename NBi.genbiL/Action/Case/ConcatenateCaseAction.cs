using NBi.GenbiL.Parser.Valuable;
using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class ConcatenateCaseAction : ICaseAction
    {
        public string ColumnName { get; private set; }
        public IEnumerable<IValuable> Valuables { get; private set; }
        public ConcatenateCaseAction(string columnName, IEnumerable<IValuable> valuables)
        {
            ColumnName = columnName;
            Valuables = valuables;
        }

        public void Execute(GenerationState state)
        {
            if (!state.TestCaseCollection.Scope.Variables.Contains(ColumnName))
                throw new ArgumentOutOfRangeException(String.Format("No column named '{0}' has been found.",ColumnName));

            var index = state.TestCaseCollection.Scope.Variables.ToList().FindIndex(v => v == ColumnName);

            foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
            {
                if ((string)row[ColumnName] != "(none)")
                    foreach (var valuable in Valuables)
                        if (valuable.GetValue(row) != "(none)")
                            row[ColumnName] = (string)row[ColumnName] + valuable.GetValue(row);
                        else
                            row[ColumnName] = "(none)";
            }
        }

        public string Display
        {
            get
            {
                return string.Format("Concatenating the content of column '{0}' with '{1}'", ColumnName, String.Join(", ", Valuables));
            }
        }
    }
}
