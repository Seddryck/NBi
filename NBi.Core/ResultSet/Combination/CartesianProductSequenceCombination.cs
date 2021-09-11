using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Combination
{
    class CartesianProductSequenceCombination
    {
        private ISequenceResolver Resolver { get; }
        public CartesianProductSequenceCombination(ISequenceResolver resolver)
            => Resolver = resolver;

        public IResultSet Execute(IResultSet rs)
        {
            var newColumn = new DataColumn($"Column{rs.Columns.Count}", typeof(object));
            rs.Columns.Add(newColumn);

            var sequence = Resolver.Execute();
            if (sequence.Count == 0 || rs.Columns.Count == 1)
            {
                rs.Clear();
            }
            else
            {
                var firstItem = sequence[0];
                foreach (DataRow row in rs.Rows)
                    row[newColumn] = firstItem;

                var newRows = new HashSet<DataRow>();
                foreach (var item in sequence.Cast<object>().Skip(1))
                {
                    foreach (DataRow row in rs.Rows)
                    {
                        var newRow = rs.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        newRow[newColumn] = item;
                        newRows.Add(newRow);
                    }
                }
                foreach (var newRow in newRows)
                    rs.Rows.Add(newRow);
            }
            rs.AcceptChanges();
            return rs;
        }
    }
}
