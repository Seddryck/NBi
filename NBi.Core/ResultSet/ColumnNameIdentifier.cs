using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class ColumnNameIdentifier : IColumnIdentifier, IEquatable<ColumnNameIdentifier>
    {
        public string Name { get; private set; }
        public string Label => $"[{Name}]";

        public ColumnNameIdentifier(string name)
            => Name = name;

        public IResultColumn GetColumn(IResultSet rs) 
            => rs.ContainsColumn(Name) ? rs.GetColumn(Name) : null;

        public object GetValue(IResultRow dataRow) => dataRow[Name];

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object value)
        {
            switch (value)
            {
                case ColumnNameIdentifier x: return Equals(x);
                default: return false;
            }
        }

        public bool Equals(ColumnNameIdentifier other)
            => !(other is null) && Name == other.Name;
    }
}
