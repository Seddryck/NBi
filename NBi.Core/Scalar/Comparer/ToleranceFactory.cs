using Expressif.Predicates.Special;
using NBi.Core.ResultSet;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer;

public class ToleranceFactory
{

    public Tolerance? Instantiate(IColumnDefinition columnDefinition)
    {
        if (string.IsNullOrEmpty(columnDefinition.Tolerance) || string.IsNullOrWhiteSpace(columnDefinition.Tolerance))
            return null;

        if (columnDefinition.Role != ColumnRole.Value)
            throw new ArgumentException("The ColumnDefinition must have have a role defined as 'Value' and is defined as 'Key'", nameof(columnDefinition));

        return Instantiate(columnDefinition.Type, columnDefinition.Tolerance);
    }

    public Tolerance? Instantiate(ColumnType type, string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            return None(type);

        return type switch
        {
            ColumnType.Text => new TextToleranceFactory().Instantiate(value),
            ColumnType.Numeric => new NumericToleranceFactory().Instantiate(value),
            ColumnType.DateTime => new DateTimeToleranceFactory().Instantiate(value),
            _ => None(type)
        };
    }
    
    public static Tolerance? None(ColumnType type)
        => type switch
        {
            ColumnType.Text => TextSingleMethodTolerance.None,
            ColumnType.Numeric => NumericAbsoluteTolerance.None,
            ColumnType.DateTime => DateTimeTolerance.None,
            _ => null
        };
}
