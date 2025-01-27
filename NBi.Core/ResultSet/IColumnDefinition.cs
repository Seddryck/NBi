using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using NBi.Extensibility;

namespace NBi.Core.ResultSet;

public interface IColumnDefinitionLight
{
    IColumnIdentifier Identifier { get; set; }
    ColumnType Type { get; set; }
}

public interface IColumnDefinition
{
    IColumnIdentifier Identifier { get; set; }
    ColumnRole Role {get; set;}
    ColumnType Type { get; set; }
    string Tolerance {get; set;}
    bool IsToleranceSpecified { get; }

    Rounding.RoundingStyle RoundingStyle { get; set; }
    string RoundingStep { get; set; }

    ITransformationInfo? Transformation { get; }
}
