using System.Data;

namespace NBi.Core.Query
{
    /// <summary>
    /// Interface defining methods implemented by engines able to parse queries
    /// </summary>
    public interface IQueryParser : IQueryEnginable
    {
        ParserResult Parse();
    }
}
