using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            else if (obj is IList<IRow>)
                return this.Build((IList<IRow>) obj);
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
            var rs = new ResultSet();
            rs.Load(ds.Tables[0]);
            return rs;
        }

        public virtual ResultSet Build(IList<IRow> rows)
        {
            var objs = new List<object[]>();
            
            foreach (var row in rows)
            {
                var cells = row.Cells.ToArray<object>();
                objs.Add(cells);
            }

            var rs = new ResultSet();
            rs.Load(objs);
            return rs;
        }
        
        public virtual ResultSet Build(string path)
        {
            var reader = new ResultSetCsvReader();
            var rs = reader.Read(path);
            return rs;
        }
    }
}
