using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnPositionIdentifier : IColumnIdentifier, IEquatable<ColumnPositionIdentifier>
    {
        public int Position { get; private set; }

        public string Label => $"#{Position.ToString()}";

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
    }
}
