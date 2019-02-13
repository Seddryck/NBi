using NBi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    abstract class CrossCaseAction : ICaseAction
    {
        public string FirstSet { get; private set; }
        public string SecondSet { get; private set; }



        public CrossCaseAction(string firstSet, string secondSet)
        {
            FirstSet = firstSet;
            SecondSet = secondSet;
        }

        public virtual void Execute(GenerationState state)
        {
            if (!state.TestCaseCollection.ItemExists(FirstSet))
                throw new ArgumentException($"The test case set named '{FirstSet}' doesn't exist.", nameof(FirstSet));

            if (!state.TestCaseCollection.ItemExists(SecondSet))
                throw new ArgumentException($"The test case set named '{SecondSet}' doesn't exist.", nameof(SecondSet));

            Cross(
                state.TestCaseCollection.Item(FirstSet).Content,
                state.TestCaseCollection.Item(SecondSet).Content,
                state.TestCaseCollection.Scope,
                MatchingRow);
        }

        public abstract bool MatchingRow(DataRow first, DataRow second);

        protected void Cross(DataTable first, DataTable second, TestCaseManager scope, Func<DataRow, DataRow, bool> matchingRow)
        {
            var table = BuildStructure(first, second);

            foreach (DataRow firstRow in first.Rows)
            {
                foreach (DataRow secondRow in second.Rows)
                {
                    if (matchingRow(firstRow, secondRow))
                    {
                        var newRow = table.NewRow();
                        foreach (DataColumn column in firstRow.Table.Columns)
                            newRow[column.ColumnName] = firstRow[column.ColumnName];
                        foreach (DataColumn column in secondRow.Table.Columns)
                            newRow[column.ColumnName] = secondRow[column.ColumnName];
                        table.Rows.Add(newRow);
                    }
                }
            }

            var dataReader = table.CreateDataReader();
            scope.Content.Clear();
            scope.Content.Load(dataReader, LoadOption.PreserveChanges);
            scope.Content.AcceptChanges();
            scope.Variables.Clear();
            foreach (DataColumn column in scope.Content.Columns)
                scope.Variables.Add(column.ColumnName);
        }

        private DataTable BuildStructure(DataTable firstSet, DataTable secondSet)
        {
            var table = new DataTable();
            foreach (DataColumn column in firstSet.Columns)
                table.Columns.Add(column.ColumnName, column.DataType);
            foreach (DataColumn column in secondSet.Columns)
                if (table.Columns.Contains(column.ColumnName))
                {
                    if (table.Columns[column.ColumnName].DataType == typeof(object) && column.DataType == typeof(string[]))
                        table.Columns[column.ColumnName].DataType = typeof(string[]);
                }
                else
                    table.Columns.Add(column.ColumnName, column.DataType);

            return table;
        }
        public abstract string Display { get; }
    }
}
