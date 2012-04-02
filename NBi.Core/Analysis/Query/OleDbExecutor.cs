using System;
using System.Data.OleDb;
using System.Data;

namespace NBi.Core.Analysis.Query
{
    public class OleDbExecutor
    {
        public string ConnectionString { get; private set; }


        public OleDbExecutor(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataSet Execute(string mdx)
        {
            // Open the connection
            using (var connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new OleDbDataAdapter(mdx, connection);
                var ds = new DataSet();

                adapter.SelectCommand.CommandTimeout = 0;
                adapter.Fill(ds);

                // capture time after execution
                long ticksAfter = DateTime.Now.Ticks;

                // setting query runtime
                int diffInSec = Convert.ToInt32((ticksAfter - ticksBefore) / 1000 / 1000);

                return ds;
            }
        }
    }
}
