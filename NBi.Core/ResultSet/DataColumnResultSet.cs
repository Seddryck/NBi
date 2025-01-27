using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

public class DataColumnResultSet(DataColumn column) : IResultColumn, IEquatable<IResultColumn>
{

    private DataColumn Column { get; } = column;
    public string Name
    { get => Column.ColumnName; }
    
    public Type DataType
    { get => Column.DataType; }

    public int Ordinal
    { get => Column.Ordinal; }

    public void Rename(string newName)
    {
        if (Column.Table?.Columns.Contains(newName) ?? throw new InvalidOperationException())
            throw new NBiException($"Cannot rename the column '{Column.ColumnName}' to '{newName}' because the table already contains a column with this name.");
        Column.ColumnName = newName;
    }

    public void Move(int ordinal)
    {
        if (ordinal >= (Column.Table?.Columns.Count ?? throw new InvalidOperationException()))
            throw new NBiException($"Cannot move the column '{Column.ColumnName}' to position '{ordinal}' because the table only contains {Column.Table.Columns.Count} columns.");
        Column.SetOrdinal(ordinal);
    }

    public void Remove()
        => (Column.Table?.Columns ?? throw new InvalidOperationException()).RemoveAt(Column.Ordinal);

    public void ReplaceBy(IResultColumn column)
    {
        var (ordinal, name) = (Column.Ordinal, Column.ColumnName);
        (Column.Table?.Columns ?? throw new InvalidOperationException()).RemoveAt(ordinal);
        column.Move(ordinal);
        column.Rename(name);
    }


    public void SetProperties(object role, object type)
        => SetProperties(role, type, null, null);
    public void SetProperties(object role, object type, object? tolerance, object? rounding)
    {
        SetProperty("Role", role);
        SetProperty("Type", type);
        SetProperty("Tolerance", tolerance);
        SetProperty("Rounding", rounding);
    }

    protected void SetProperty(string property, object? value)
    {
        if (value != null)
        { 
            if (Column.ExtendedProperties.ContainsKey($"NBi::{property}"))
                Column.ExtendedProperties[$"NBi::{property}"] = value;
            else
                Column.ExtendedProperties.Add($"NBi::{property}", value);
        }
    }

    public bool HasProperties()
        => Column.ExtendedProperties.Count > 0;

    public object? GetProperty(string property)
    {
        if (Column.ExtendedProperties.ContainsKey($"NBi::{property}"))
            return Column.ExtendedProperties[$"NBi::{property}"];
        else
            return null;
    }

    public override int GetHashCode() 
        => Name.GetHashCode() ^ 37 * Ordinal.GetHashCode();

    public override bool Equals(object? other)
        => other is DataColumnResultSet && Equals(other as DataColumnResultSet);
    
    public bool Equals(IResultColumn? other)
        => other is not null
            && (
                ReferenceEquals(this, other)
                || (
                    Name.Equals(other.Name)
                    && Ordinal.Equals(other.Ordinal)
                )
            );

    public static bool operator == (DataColumnResultSet obj1, IResultColumn obj2)
    {
        if (ReferenceEquals(obj1, obj2))
            return true;
        
        if (obj1 is null || obj2 is null)
            return false;

        return obj1.Equals(obj2);
    }

    public static bool operator != (DataColumnResultSet obj1, IResultColumn obj2)
        => !(obj1 == obj2);
}
