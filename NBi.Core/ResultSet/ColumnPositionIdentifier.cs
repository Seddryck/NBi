using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.ResultSet
{
    class ColumnPositionIdentifier : IColumnIdentifier, IEquatable<ColumnPositionIdentifier>
    {
        public int Position { get; protected set; }

        public virtual string Label => $"#{Position.ToString()}";

        public ColumnPositionIdentifier(int position)
        {
            Position = position;
        }

        public override bool Equals(object obj) => this.Equals(obj as ColumnPositionIdentifier);
        public override int GetHashCode() => Position.GetHashCode();

        public bool Equals(ColumnPositionIdentifier other)
        {
            if (other is null)
                return false;
            return (other.Position == Position);
        }

        public IResultColumn GetColumn(IResultSet rs)
            => rs.GetColumn(Position);
        public object GetValue(IResultRow dataRow)
            => dataRow[Position];
    }
}
