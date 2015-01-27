using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    abstract class CrossAbstractCaseAction : ICaseAction
    {
        public string FirstSet { get; set; }

        public CrossAbstractCaseAction(string firstSet)
        {
            FirstSet = firstSet;
        }

        public void Execute(GenerationState state)
        {
            ExecutePreChecks(state);
            
            var dataReader = CrossContent(state);

            var scope = state.TestCaseSetCollection.Scope;
            scope.Content.Clear();
            scope.Content.Load(dataReader, LoadOption.PreserveChanges);
            scope.Content.AcceptChanges();
        }

        protected virtual void ExecutePreChecks(GenerationState state)
        {
            if (!state.TestCaseSetCollection.ItemExists(FirstSet))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", FirstSet), "firstSet");
        }

        protected abstract IDataReader CrossContent(GenerationState state);


        protected IDataReader CrossContent(DataTable first, DataTable second, Func<DataRow, DataRow, bool> matchingRow)
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

            return table.CreateDataReader();
        }

        private DataTable BuildStructure(DataTable firstSet, DataTable secondSet)
        {
            var table = new DataTable();
            foreach (DataColumn column in firstSet.Columns)
                table.Columns.Add(column.ColumnName);
            foreach (DataColumn column in secondSet.Columns)
                if (!table.Columns.Contains(column.ColumnName))
                    table.Columns.Add(column.ColumnName);

            return table;
        }

        public abstract string Display { get; }
    }
}
