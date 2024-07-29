using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence
{
    public interface IEquivaler
    {
        ResultResultSet Compare(IResultSet x, IResultSet y);
        ISettingsResultSet? Settings { get; }
        EngineStyle Style { get; }
    }
}
