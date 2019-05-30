using NBi.GenbiL.Parser.Valuable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class SplitCaseAction : ICaseAction
    {
        public IEnumerable<string> Columns { get; private set; }
        public string Separator { get; private set; }
        public SplitCaseAction(IEnumerable<string> columns, string separator)
        {
            Columns = columns;
            Separator = separator;
        }

        public void Execute(GenerationState state)
        {
            var dataTable = state.TestCaseCollection.Scope.Content;

            foreach (var columnName in Columns)
            {
                if (!state.TestCaseCollection.Scope.Variables.Contains(columnName))
                    throw new ArgumentOutOfRangeException(String.Format("No column named '{0}' has been found.", columnName));

                dataTable.Columns.Add("_" + columnName, typeof(string[]));


                var columnListId = dataTable.Columns["_" + columnName].Ordinal;
                var columnId = dataTable.Columns[columnName].Ordinal;

                for (int i = 0; i < dataTable.Rows.Count; i++)
			    {
                    if (dataTable.Rows[i][columnId] is IEnumerable<string>)
                        throw new ArgumentOutOfRangeException(String.Format("The column named '{0}' was already an array.", columnName));

                    var array = dataTable.Rows[i][columnId].ToString().Split(new[] { Separator }, StringSplitOptions.None);
                    dataTable.Rows[i][columnListId] = array;
                }

                dataTable.Columns["_" + columnName].SetOrdinal(columnId);
                dataTable.Columns.Remove(columnName);
                dataTable.Columns["_" + columnName].ColumnName = columnName;
            }

            dataTable.AcceptChanges();
        }

        public string Display
        {
            get
            {
                return string.Format("Splitting the content of columns '{0}' based on separator '{1}'", String.Join(", ", Columns), Separator);
            }
        }
    }
}
