using System.Collections;

namespace NBi.Core.ResultSet
{
    public interface IResultSetComparer
    {
        ResultSetCompareResult Compare(object x, object y);
        ResultSetComparisonSettings Settings { get; set; }
    }
}
