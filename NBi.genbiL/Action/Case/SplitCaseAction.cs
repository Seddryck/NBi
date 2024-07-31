using NBi.GenbiL.Parser.Valuable;
using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class SplitCaseAction : ISingleCaseAction
    {
        public IEnumerable<string> Columns { get; private set; }
        public string Separator { get; private set; }
        public SplitCaseAction(IEnumerable<string> columns, string separator)
        {
            Columns = columns;
            Separator = separator;
        }

        public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

        public void Execute(CaseSet testCases)
        {
            var dataTable = testCases.Content;

            foreach (var columnName in Columns)
            {
                if (!testCases.Variables.Contains(columnName))
                    throw new ArgumentOutOfRangeException($"No column named '{columnName}' has been found.");

                dataTable.Columns.Add("_" + columnName, typeof(string[]));


                var columnListId = dataTable.Columns["_" + columnName]!.Ordinal;
                var columnId = dataTable.Columns[columnName]!.Ordinal;

                for (int i = 0; i < dataTable.Rows.Count; i++)
			    {
                    if (dataTable.Rows[i][columnId] is IEnumerable<string>)
                        throw new ArgumentOutOfRangeException($"The column named '{columnName}' was already an array.");

                    var array = dataTable.Rows[i][columnId]!.ToString()!.Split(new[] { Separator }, StringSplitOptions.None);
                    dataTable.Rows[i][columnListId] = array;
                }

                dataTable.Columns["_" + columnName]!.SetOrdinal(columnId);
                dataTable.Columns.Remove(columnName);
                dataTable.Columns["_" + columnName]!.ColumnName = columnName;
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
