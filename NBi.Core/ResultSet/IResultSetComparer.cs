using System.Collections;

namespace NBi.Core.ResultSet
{
    public interface IResultSetComparer
    {
        int Compare(object x, object y);
    }
}
