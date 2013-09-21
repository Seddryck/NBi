using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
    public interface IColumnDefinition
    {
        int Index { get; set; }
        ColumnRole Role {get; set;}
        ColumnType Type { get; set; }
        string Tolerance {get; set;}
        bool IsToleranceSpecified { get; }

        Rounding.RoundingStyle RoundingStyle { get; set; }
        string RoundingStep { get; set; }
    }
}
