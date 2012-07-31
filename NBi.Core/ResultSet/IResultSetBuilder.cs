using System.Data;

namespace NBi.Core.ResultSet
{
    public interface IResultSetBuilder
    {
        ResultSet Build(object obj);
    }
}
