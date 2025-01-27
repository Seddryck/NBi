using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.ResultSet;

class ColumnPositionIdentifier(int position) : IColumnIdentifier, IEquatable<ColumnPositionIdentifier>
{
    public int Position { get; protected set; } = position;

    public virtual string Label => $"#{Position}";

    public override bool Equals(object? obj) 
        => Equals(obj as ColumnPositionIdentifier);

    public override int GetHashCode() => Position.GetHashCode();

    public bool Equals(ColumnPositionIdentifier? other)
        => other is not null && other.Position == Position;

    public IResultColumn GetColumn(IResultSet rs)
        => rs.GetColumn(Position) ?? throw new NullReferenceException();
    public object? GetValue(IResultRow dataRow)
        => dataRow[Position];
}
