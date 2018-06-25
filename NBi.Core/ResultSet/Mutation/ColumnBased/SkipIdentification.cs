using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Mutation.ColumnBased
{
    class SkipIdentification
    {
        private IEnumerable<IColumnIdentifier> Identifiers { get; }

        public SkipIdentification(IEnumerable<IColumnIdentifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public void Execute(ResultSet resultSet)
        {
            var factory = new ColumnIdentifierFactory();
            var columns = new List<DataColumn>();
            foreach (var identifier in Identifiers)
            {
                if (identifier is ColumnPositionIdentifier)
                    if ((identifier as ColumnPositionIdentifier).Position < resultSet.Columns.Count)
                        columns.Add(resultSet.Columns[(identifier as ColumnPositionIdentifier).Position]);
                if (identifier is ColumnNameIdentifier)
                    if (resultSet.Columns.Contains((identifier as ColumnNameIdentifier).Name))
                        columns.Add(resultSet.Columns[(identifier as ColumnNameIdentifier).Name]);
            }

            var i = 0;
            while (i< resultSet.Columns.Count)
            {
                if (IsColumnToRemove(resultSet, columns, i))
                    resultSet.Columns.RemoveAt(i);
                else
                    i++;
            }
            resultSet.Table.AcceptChanges();
        }

        protected virtual bool IsColumnToRemove(ResultSet resultSet, IEnumerable<DataColumn> identifiedColumns, int index)
            => identifiedColumns.Contains(resultSet.Columns[index]);


    }
}
