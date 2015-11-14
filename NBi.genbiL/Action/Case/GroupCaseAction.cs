using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class GroupCaseAction : ICaseAction
    {
        public List<string> columnNames { get; set; }

        public GroupCaseAction(IEnumerable<string> variableNames)
        {
            this.columnNames = new List<string>(variableNames);
        }

        public void Execute(GenerationState state)
        {
            foreach (var columnName in columnNames)
            {
                var c = new DataColumn("_" + columnName, typeof(List<string>));
                //c.DefaultValue = new List<string>();
                state.TestCaseCollection.Scope.Content.Columns.Add(c);
            }


            var dataTable = state.TestCaseCollection.Scope.Content;

            int i = 0;
            var firstRow = 0;
            while(i < dataTable.Rows.Count)
            {
                var isIdentical = true;
                for (int j = 0; j < dataTable.Columns.Count - columnNames.Count; j++)
                {
                    if (!columnNames.Contains(dataTable.Columns[j].ColumnName))
                        isIdentical &= dataTable.Rows[i][j] == dataTable.Rows[firstRow][j];
                }

                if (!isIdentical)
                    firstRow = i;
                
                foreach (var columnName in columnNames)
                {
                    var columnListId =  dataTable.Columns["_" + columnName].Ordinal;
                    var columnId = dataTable.Columns[columnName].Ordinal;

                    if (dataTable.Rows[firstRow][columnListId] == DBNull.Value)
                        dataTable.Rows[firstRow][columnListId] = new List<string>();

                    var list = dataTable.Rows[firstRow][columnListId] as IList<string>;
                    list.Add(dataTable.Rows[i][columnId].ToString());
                }

                if (isIdentical && i!=0)
                    dataTable.Rows[i].Delete();
                else
                    i++;
            }

            foreach (var columnName in columnNames)
            {
                var columnId = dataTable.Columns[columnName].Ordinal;

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
                if (columnNames.Count == 1)
                    return string.Format("Grouping rows for content of column '{0}'", columnNames[0]);
                else
                    return string.Format("Grouping rows for content of column '{0}'", String.Join("', '", columnNames));
            }
        }
    }
}
