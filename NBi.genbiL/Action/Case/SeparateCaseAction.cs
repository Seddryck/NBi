using NBi.GenbiL.Parser.Valuable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class SeparateCaseAction : ICaseAction
    {
        public string ColumnName { get; private set; }
        public IEnumerable<string> NewColumns { get; private set; }
        public string Separator { get; private set; }
        public SeparateCaseAction(string columnName, IEnumerable<string> newColumns, string separator)
        {
            ColumnName = columnName;
            NewColumns = newColumns;
            Separator = separator;
        }

        public void Execute(GenerationState state)
        {
            if (!state.TestCaseCollection.Scope.Variables.Contains(ColumnName))
                throw new ArgumentOutOfRangeException(String.Format("No column named '{0}' has been found.",ColumnName));

            var index = state.TestCaseCollection.Scope.Variables.ToList().FindIndex(v => v == ColumnName);

            foreach (var newColumnName in NewColumns)
            {
                if (state.TestCaseCollection.Scope.Variables.Contains(newColumnName))
                    throw new ArgumentException(String.Format("Column '{0}' already existing.", newColumnName));

                state.TestCaseCollection.Scope.Variables.Add(newColumnName);
                var newColumn = new DataColumn(newColumnName);
                state.TestCaseCollection.Scope.Content.Columns.Add(newColumn);
            }

            foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
            {
                if ((string)row[ColumnName] != "(none)")
                {
                    var value = (string)row[ColumnName];
                    var array = value.Split(new string[] { Separator }, NewColumns.Count(), StringSplitOptions.None);

                    var i = 0;
                    foreach (var newColumnName in NewColumns)
                    {
                        if (i>=array.Length || string.IsNullOrEmpty(array[i]))
                            row[newColumnName] = "(none)";
                        else
                            row[newColumnName] = array[i];
                        i++;
                    }
                }
            }
        }

        public string Display
        {
            get
            {
                return string.Format("Separating the content of column '{0}' into '{1}' based on separator '{2}'", ColumnName, String.Join(", ", NewColumns), Separator);
            }
        }
    }
}
