using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    abstract class CrossCaseAction : IMultiCaseAction
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
            if (!state.CaseCollection.TryGetValue(FirstSet, out var first))
                throw new ArgumentException($"The test case set named '{FirstSet}' doesn't exist.", nameof(FirstSet));

            if (!state.CaseCollection.TryGetValue(SecondSet, out var second))
                throw new ArgumentException($"The test case set named '{SecondSet}' doesn't exist.", nameof(SecondSet));

            Cross(
                first.Content,
                second.Content,
                state.CaseCollection.CurrentScope,
                MatchingRow);
        }

        public abstract bool MatchingRow(DataRow first, DataRow second);

        protected void Cross(DataTable first, DataTable second, CaseSet destination, Func<DataRow, DataRow, bool> matchingRow)
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
            destination.Content.Clear();
            destination.Content.Load(dataReader, LoadOption.PreserveChanges);
            destination.Content.AcceptChanges();
        }

        protected virtual DataTable BuildStructure(DataTable firstSet, DataTable secondSet)
        {
            var table = new DataTable();
            foreach (DataColumn column in firstSet.Columns)
                table.Columns.Add(column.ColumnName, column.DataType);
            foreach (DataColumn column in secondSet.Columns)
                if (table.Columns.Contains(column.ColumnName))
                {
                    if (table.Columns[column.ColumnName]!.DataType == typeof(object) && column.DataType == typeof(string[]))
                        table.Columns[column.ColumnName]!.DataType = typeof(string[]);
                }
                else
                    table.Columns.Add(column.ColumnName, column.DataType);

            return table;
        }
        public abstract string Display { get; }
    }
}
