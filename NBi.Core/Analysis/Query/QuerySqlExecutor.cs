using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace NBi.Core.Analysis.Query
{
    public class QuerySqlExecutor:IQueryExecutor
    {
        public string ConnectionString { get; private set; }


        public QuerySqlExecutor(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataSet Execute(string mdx)
        {
            int i;
            return Execute(mdx, out i);
        }

        public DataSet Execute(string mdx, out int elapsedSec)
        {
            // Open the connection
            using (var connection = new SqlConnection())
            {
                try
                    {connection.ConnectionString = ConnectionString;}
                catch (ArgumentException ex)
                    {throw new ConnectionException(ex);}

                try
                    {connection.Open();}
                catch (SqlException ex)
                    {throw new ConnectionException(ex);}

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new SqlDataAdapter(mdx, connection);
                var ds = new DataSet();
                
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.Fill(ds);

                // capture time after execution
                long ticksAfter = DateTime.Now.Ticks;

                // setting query runtime
                elapsedSec = Convert.ToInt32((ticksAfter - ticksBefore) / 1000 / 1000);

                return ds;
            }
        }
    }
}
