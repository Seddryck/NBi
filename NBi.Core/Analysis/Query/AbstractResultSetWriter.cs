using System;
using System.Data;

namespace NBi.Core.Analysis.Query
{
    public abstract class AbstractResultSetWriter
    {
        public string PersistencePath { get; private set; }

        public AbstractResultSetWriter(string persistancePath)
        {
            PersistencePath = persistancePath;
        }

        public void Write(string filename, DataSet ds)
        {
            if (ds.Tables.Count == 0)
                throw new Exception("The DataSet contains no table");

            if (ds.Tables.Count > 1)
                throw new Exception("The DataSet has more than one table");

            OnWrite(filename, ds, ds.Tables[0].TableName);
        }

        public void Write(string filename, DataSet ds, string tableName)
        {
            if (!ds.Tables.Contains(tableName))
                throw new Exception(string.Format("The dataset doesn't contain a table named '{0}'", tableName));

            OnWrite(filename, ds, ds.Tables[0].TableName);
        }


        protected abstract void OnWrite(string filename, DataSet ds, string tableName);
    }
}
