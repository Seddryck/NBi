using System.Data;

namespace NBi.Core.ResultSet
{
    public interface IResultSetWriter
    {
        string PersistencePath { get; set; }
        void Write(string filename, DataSet dataset);
    }
}
