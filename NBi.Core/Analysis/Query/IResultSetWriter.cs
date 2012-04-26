using System.Data;

namespace NBi.Core.Analysis.Query
{
    public interface IResultSetWriter
    {
        string PersistencePath { get; set; }
        void Write(string filename, DataSet dataset);
    }
}
