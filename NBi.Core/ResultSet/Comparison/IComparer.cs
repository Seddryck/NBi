namespace NBi.Core.ResultSet.Comparison
{
    public interface IComparer
    {
        ResultResultSet Compare(object x, object y);
        ISettingsResultSet Settings { get; set; }
        EngineStyle Style { get; }
    }
}
