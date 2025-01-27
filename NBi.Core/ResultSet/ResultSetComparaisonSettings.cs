using NBi.Extensibility;
using NCalc.Domain;
using System.Xml.Serialization;

namespace NBi.Core.ResultSet;

public class ResultSetComparaisonSettings
{
    public enum KeysChoice
    {
        [XmlEnum(Name = "first")]
        First = 0,
        [XmlEnum(Name = "all-except-last")]
        AllExpectLast = 1,
        [XmlEnum(Name = "all")]
        All = 2
    }

    public enum ValuesChoice
    {
        [XmlEnum(Name = "all-except-first")]
        AllExpectFirst = 0,
        [XmlEnum(Name = "last")]
        Last = 1
    }

    private KeysChoice KeysDef { get; set; }
    private ValuesChoice ValuesDef { get; set; }
    private ICollection<IColumnDefinition> ColumnsDef { get; set; }
    private decimal DefaultTolerance { get; set; }

    public bool IsKey(int index)
    {
        if (ColumnsDef.Any( c => c.Identifier==new ColumnOrdinalIdentifier(index) && c.Role!=ColumnRole.Key))
            return false;
        
        if (ColumnsDef.Any( c => c.Identifier == new ColumnOrdinalIdentifier(index) && c.Role==ColumnRole.Key))
            return true;

        return KeysDef switch
        {
            KeysChoice.First => index == 0,
            KeysChoice.AllExpectLast => index != GetLastColumnIndex(),
            KeysChoice.All => true,
            _ => false,
        };
    }

    public bool IsValue(int index)
    {
        if (ColumnsDef.Any(c => c.Identifier == new ColumnOrdinalIdentifier(index) && c.Role != ColumnRole.Value))
            return false;

        if (ColumnsDef.Any(c => c.Identifier == new ColumnOrdinalIdentifier(index) && c.Role == ColumnRole.Value))
            return true;

        return ValuesDef switch
        {
            ValuesChoice.AllExpectFirst => index != 0,
            ValuesChoice.Last => index == GetLastColumnIndex(),
            _ => false,
        };
    }

    public ColumnRole GetColumnRole(int index)
    {
        if (IsKey(index))
            return ColumnRole.Key;
        else if (IsValue(index))
            return ColumnRole.Value;
        else
            return ColumnRole.Ignore;
    }

    public ColumnType GetColumnType(int index)
    {
        if (IsNumeric(index))
            return ColumnType.Numeric;
        else
            return ColumnType.Text;
    }

    public bool IsNumeric(int index)
    {
        if (ColumnsDef.Any(c => c.Identifier == new ColumnOrdinalIdentifier(index) && c.Type != ColumnType.Numeric))
            return false;

        if (ColumnsDef.Any(c => c.Identifier == new ColumnOrdinalIdentifier(index) && c.Type == ColumnType.Numeric))
            return true;

        return IsValue(index);
    }

    public decimal GetTolerance(int index)
    {
        var col = ColumnsDef.FirstOrDefault(c => c.Identifier == new ColumnOrdinalIdentifier(index));
        return col == null ? DefaultTolerance : Convert.ToDecimal(col.Tolerance);
    }

    public int GetLastColumnIndex()
        => _lastColumnIndex;

    public int GetLastKeyColumnIndex()
    {
        var max = 0;
        for (int i = 0; i < GetLastColumnIndex(); i++)
            if (IsKey(i))
                max = i;

        return max;
    }

    protected int _lastColumnIndex;

    public void ApplyTo(int columnCount)
    {
        _lastColumnIndex = columnCount-1;
    }
    
    public ResultSetComparaisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, ICollection<IColumnDefinition> columnsDef)
        : this(keysDef, valuesDef, 0, columnsDef)
    { }

    public ResultSetComparaisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, decimal defaultTolerance)
        : this(keysDef, valuesDef, defaultTolerance, [])
    { }

    public ResultSetComparaisonSettings(KeysChoice keysDef, ValuesChoice valuesDef, decimal defaultTolerance, ICollection<IColumnDefinition> columnsDef)
    {
        KeysDef = keysDef;
        ValuesDef = valuesDef;
        DefaultTolerance = defaultTolerance;
        if (columnsDef != null)
            ColumnsDef = columnsDef;
        else
            ColumnsDef = [];
    }
}
