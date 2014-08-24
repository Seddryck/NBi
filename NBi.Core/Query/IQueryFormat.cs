using System.Data;

namespace NBi.Core.Query
{
    /// <summary>
    /// Interface defining methods implemented by engines able to get the formats of returned cells
    /// </summary>
    public interface IQueryFormat : IQueryEnginable
    {
        FormattedResults GetFormats();
    }
}
