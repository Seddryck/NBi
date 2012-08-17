namespace NBi.Core.ResultSet
{
    public interface IColumn
    {
        int Index { get; set; }
        ColumnRole Role {get; set;}
        ColumnType Type { get; set; }
        decimal Tolerance {get; set;}
        bool IsToleranceSpecified { get; set; }
    }
}
