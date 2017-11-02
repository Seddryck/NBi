namespace NBi.Core.ResultSet.Comparison
{
    public interface IComparerResultSet
    {
        ResultResultSet Compare(object x, object y);
        ISettingsResultSet Settings { get; set; }
        ComparisonStyle Style { get; }
    }
}
