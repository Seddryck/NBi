using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class GroupCaseAction : ISingleCaseAction
    {
        public List<string> ColumnNames { get; }

        public GroupCaseAction(IEnumerable<string> variableNames)
        {
            this.ColumnNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

        public void Execute(CaseSet testCases)
        {
            var dataTable = testCases.Content;
            dataTable.AcceptChanges();

            foreach (var columnName in ColumnNames)
                dataTable.Columns.Add($"_{columnName}", typeof(List<string>));

            int i = 0;
            var firstRow = 0;
            while(i < dataTable.Rows.Count)
            {
                var isIdentical = true;
                for (int j = 0; j < dataTable.Columns.Count - ColumnNames.Count; j++)
                {
                    if (!ColumnNames.Contains(dataTable.Columns[j].ColumnName) && dataTable.Rows[i][j] is not IEnumerable<string>)
                        isIdentical &= dataTable.Rows[i][j].ToString() == dataTable.Rows[firstRow][j].ToString();
                }

                if (!isIdentical)
                    firstRow = i;
                
                foreach (var columnName in ColumnNames)
                {
                    var columnListId =  dataTable.Columns[$"_{columnName}"]!.Ordinal;
                    var columnId = dataTable.Columns[columnName]!.Ordinal;

                    if (dataTable.Rows[firstRow][columnListId] == DBNull.Value)
                        dataTable.Rows[firstRow][columnListId] = new List<string>();

                    var list = (List<string>)dataTable.Rows[firstRow][columnListId];
                    if (dataTable.Rows[i][columnId] is IEnumerable<string> strings)
                        list.AddRange(strings);
                    else
                        list.Add(dataTable.Rows[i]![columnId]!.ToString()!);
                }

                if (isIdentical && i != 0)
                    dataTable.Rows[i].Delete();
                i++;
            }


            foreach (var columnName in ColumnNames)
                dataTable.Columns.Add($"_{columnName}_", typeof(string[]));

            foreach (DataRow row in dataTable.Rows.Cast<DataRow>().Where(x => x.RowState!=DataRowState.Deleted))
                foreach (var columnName in ColumnNames)
                    row[$"_{columnName}_"] = ((List<string>)row[$"_{columnName}"]).ToArray();

            foreach (var columnName in ColumnNames)
            {
                var columnId = dataTable.Columns[columnName]!.Ordinal;

                dataTable.Columns[$"_{columnName}_"]!.SetOrdinal(columnId);
                dataTable.Columns.Remove(columnName);
                dataTable.Columns.Remove($"_{columnName}");
                dataTable.Columns[$"_{columnName}_"]!.ColumnName = columnName;
            }

            dataTable.AcceptChanges();
            
        }

        public string Display
        {
            get
            {
                if (ColumnNames.Count == 1)
                    return string.Format("Grouping rows for content of column '{0}'", ColumnNames[0]);
                else
                    return string.Format("Grouping rows for content of columns '{0}'", String.Join("', '", ColumnNames));
            }
        }
    }
}
