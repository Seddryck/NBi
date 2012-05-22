using System;
using System.Data;
using System.Data.OleDb;

namespace NBi.Core.Query
{
    public class QueryOleDbEngine:IQueryExecutor, IQueryPerformance, IQueryParser, IQueryEnginable
    {
        public string ConnectionString { get; private set; }


        public QueryOleDbEngine(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public ParserResult Parse(IDbCommand cmd)
        {
            ParserResult res=null;
            
            using(var conn = new OleDbConnection(cmd.Connection.ConnectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", cmd.CommandText);
                
                conn.Open();

                using (var cmdIn = new OleDbCommand(fullSql, conn))
                {
                    try 
	                {
                        cmdIn.ExecuteNonQuery();
                        res = ParserResult.NoParsingError();
	                }
	                catch (OleDbException ex)
	                {
                        res = new ParserResult(ex.Message.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
	                }
                    
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                conn.Close();
            }

            return res;
        }

        public PerformanceResult CheckPerformance(IDbCommand cmd, bool cleanCache)
        {
            DateTime tsStart, tsStop;

            if (cleanCache)
            {
                using (var conn = new OleDbConnection(cmd.Connection.ConnectionString))
                {
                    var clearSql = new string[] { "dbcc freeproccache", "dbcc dropcleanbuffers" };

                    conn.Open();

                    foreach (var sql in clearSql)
                    {
                        using (var cleanCmd = new OleDbCommand(sql, conn))
                        {
                            cleanCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            tsStart = DateTime.Now;
            cmd.ExecuteNonQuery();
            tsStop = DateTime.Now;

            if (cmd.Connection.State == ConnectionState.Open)
                cmd.Connection.Close();

            return new PerformanceResult(tsStop.Subtract(tsStart));
        }

        public DataSet Execute(string mdx)
        {
            int i;
            return Execute(mdx, out i);
        }

        public DataSet Execute(string mdx, out int elapsedSec)
        {
            // Open the connection
            using (var connection = new OleDbConnection())
            {
                try
                    {connection.ConnectionString = ConnectionString;}
                catch (ArgumentException ex)
                    {throw new ConnectionException(ex);}

                try
                    {connection.Open();}
                catch (OleDbException ex)
                    {throw new ConnectionException(ex);}

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new OleDbDataAdapter(mdx, connection);
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
