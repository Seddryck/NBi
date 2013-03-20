using System;
using System.Data;
using System.Data.SqlClient;

namespace NBi.Core.Query
{
    public class QuerySqlEngine:IQueryExecutor, IQueryPerformance, IQueryParser, IQueryEnginable
    {
        protected readonly SqlCommand _command;


        public QuerySqlEngine(SqlCommand cmd)
        {
            _command = cmd;
        }

        public void CleanCache()
        {
            
            using (var conn = new SqlConnection(_command.Connection.ConnectionString))
            {
                var clearSql = new string[] { "dbcc freeproccache", "dbcc dropcleanbuffers" };

                conn.Open();

                foreach (var sql in clearSql)
                {
                    using (SqlCommand cleanCmd = new SqlCommand(sql, conn))
                    {
                        cleanCmd.ExecuteNonQuery();
                    }
                }
            }
            
        }

        public PerformanceResult CheckPerformance()
        {
            DateTime tsStart, tsStop;

            if (_command.Connection.State == ConnectionState.Closed)
                _command.Connection.Open();

            tsStart = DateTime.Now;
            _command.ExecuteNonQuery();
            tsStop = DateTime.Now;

            if (_command.Connection.State == ConnectionState.Open)
                _command.Connection.Close();

            return new PerformanceResult(tsStop.Subtract(tsStart));

        }

        public ParserResult Parse()
        {
            ParserResult res = null;

            using (var conn = new SqlConnection(_command.Connection.ConnectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", _command.CommandText);

                conn.Open();

                using (SqlCommand cmdIn = new SqlCommand(fullSql, conn))
                {
                    try
                    {
                        cmdIn.ExecuteNonQuery();
                        res = ParserResult.NoParsingError();
                    }
                    catch (SqlException ex)
                    {
                        res = new ParserResult(ex.Message.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
                    }

                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            return res;
        }

        public DataSet Execute()
        {
            int i;
            return Execute(out i);
        }

        public DataSet Execute(out int elapsedSec)
        {
            // Open the connection
            using (var connection = new SqlConnection())
            {
                var connectionString = _command.Connection.ConnectionString;
                try
                { connection.ConnectionString = connectionString; }
                catch (ArgumentException ex)
                { throw new ConnectionException(ex, connectionString); }
                try
                    {connection.Open();}
                catch (SqlException ex)
                { throw new ConnectionException(ex, connectionString); }

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new SqlDataAdapter(_command.CommandText, connection);
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
