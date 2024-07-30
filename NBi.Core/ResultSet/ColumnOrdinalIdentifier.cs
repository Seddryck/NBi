using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnOrdinalIdentifier(int position) : IColumnIdentifier
    {
        public int Ordinal { get; protected set; } = position;

        public virtual string Label => $"#{Ordinal}";

        public IResultColumn? GetColumn(IResultSet rs) 
            => Ordinal < rs.ColumnCount ? rs.GetColumn(Ordinal) : null;

        public object? GetValue(IResultRow dataRow) => dataRow[Ordinal];

        public override int GetHashCode() => Ordinal.GetHashCode();

        public override bool Equals(object? value)
        {
            return !(value is not ColumnOrdinalIdentifier columnOrdinalIdentifier)
                && Ordinal == columnOrdinalIdentifier.Ordinal;
        }
    }
}
