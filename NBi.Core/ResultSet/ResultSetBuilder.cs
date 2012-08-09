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
            Console.WriteLine("Debug: {0} {1}", obj.GetType(), obj.ToString()); 
            
            if (obj is ResultSet)
                return Build((ResultSet)obj);
            else if (obj is IList<IRow>)
                return Build((IList<IRow>)obj);
            else if (obj is IDbCommand)
                return Build((IDbCommand)obj);
            else if (obj is string)
                return Build((string)obj);

            throw new ArgumentOutOfRangeException(string.Format("Type '{0}' is not expected when building a ResultSet", obj.GetType()));
        }
        
        public virtual ResultSet Build(ResultSet resultSet)
        {
            return resultSet;
        }

        public virtual ResultSet Build(IList<IRow> rows)
        {
            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
        
        public virtual ResultSet Build(IDbCommand cmd)
        {
            var qe = new QueryEngineFactory().GetExecutor(cmd);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
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
