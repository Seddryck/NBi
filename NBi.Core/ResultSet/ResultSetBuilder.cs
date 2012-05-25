using System;
using System.Data;
using NBi.Core.Query;

namespace NBi.Core.ResultSet
{
    public class ResultSetBuilder : IResultSetBuilder
    {
        public virtual ResultSet Build(Object obj)
        {
            if (obj is IDbCommand)
                return this.Build((IDbCommand)obj);
            else if (obj is DataSet)
                return this.Build((DataSet)obj);
            else if (obj is string)
                return this.Build((string)obj);
            else if (obj is ResultSet)
                return (ResultSet)obj;

            throw new ArgumentException();
        }

        
        public virtual ResultSet Build(IDbCommand cmd)
        {
            var ds = new QueryEngineFactory().GetExecutor(cmd).Execute();
            return Build(ds);
        }

        public virtual ResultSet Build(DataSet ds)
        {
            var rsw = new ResultSetCsvWriter(string.Empty);
            var rs = rsw.BuildContent(ds.Tables[0]);
            return new ResultSet(rs);
        }

        public virtual ResultSet Build(string path)
        {
            var reader = new ResultSetCsvReader(path);
            var rs = reader.Read(string.Empty);
            return rs;
        }
    }
}
