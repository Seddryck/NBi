using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnOrdinalIdentifier : IColumnIdentifier
    {
        public int Ordinal { get; protected set; }

        public virtual string Label => $"#{Ordinal.ToString()}";

        public ColumnOrdinalIdentifier(int position)
        {
            Ordinal = position;
        }

        public DataColumn GetColumn(IResultSet rs) 
            => Ordinal < rs.Columns.Count ? rs.Columns[Ordinal] : null;

        public DataColumn GetColumn(DataTable dataTable)
            => Ordinal < dataTable.Columns.Count ? dataTable.Columns[Ordinal] : null;

        public object GetValue(DataRow dataRow) => dataRow[Ordinal];

        public override int GetHashCode() => Ordinal.GetHashCode();

        public override bool Equals(object value)
        {
            var columnOrdinalIdentifier = value as ColumnOrdinalIdentifier;

            return !(columnOrdinalIdentifier is null)
                && Ordinal==columnOrdinalIdentifier.Ordinal;
        }
    }
}
