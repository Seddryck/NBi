using NBi.Core.ResultSet;

namespace NBi.Model.Items.ResultSet;

public class Cell : ICell
{
    public string? StringValue { get; set; }

    public object Value { get => StringValue!; set { StringValue = value.ToString(); } }

    public string? ColumnName { get; set; }

    public override string ToString() => StringValue?.ToString() ?? string.Empty;
}
