using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnNameIdentifier : IColumnIdentifier, IEquatable<ColumnNameIdentifier>
    {
        public string Name { get; private set; }
        public string Label => $"[{Name}]";

        public ColumnNameIdentifier(string name)
        {
            Name = name;
        }


        public override bool Equals(object obj) => this.Equals(obj as ColumnNameIdentifier);

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(ColumnNameIdentifier other)
        {
            if (other is null)
                return false;
            return (other.Name == Name);
        }
    }
}
