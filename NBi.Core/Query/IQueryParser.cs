using System.Data;

namespace NBi.Core.Query
{
    public interface IQueryParser : IQueryEnginable
    {
        ParserResult Parse();
    }
}
