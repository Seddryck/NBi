namespace NBi.Core.ResultSet.Equivalence
{
    public interface IEquivaler
    {
        ResultResultSet Compare(object x, object y);
        ISettingsResultSet Settings { get; set; }
        EngineStyle Style { get; }
    }
}
