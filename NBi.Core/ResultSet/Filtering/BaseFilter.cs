using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public abstract class BaseFilter : IResultSetFilter
    {
        protected Context Context { get; }
        protected BaseFilter(Context context)
        => Context = context;

        public IResultSet AntiApply(IResultSet rs) => Apply(rs, (x => !x));

        public IResultSet Apply(IResultSet rs) => Apply(rs, (x => x));

        protected IResultSet Apply(IResultSet rs, Func<bool, bool> onApply)
        {
            var table = rs.Clone();
            table.Clear();

            foreach (DataRow row in rs.Rows)
            {
                Context.Switch(row);
                if (onApply(RowApply(Context)))
                {
                    if (table.Rows.Count == 0 && table.Columns.Count != row.Table.Columns.Count)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            if (!table.Columns.Cast<DataColumn>().Any(x => x.ColumnName == column.ColumnName))
                                table.Columns.Add(column.ColumnName, typeof(object));
                        }
                    }
                    table.ImportRow(row);
                }
            }

            table.AcceptChanges();
            return table;
        }

        protected abstract bool RowApply(Context context);

        public abstract string Describe();
    }
}