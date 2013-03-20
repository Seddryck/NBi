using System;
using System.Data;
using System.Data.OleDb;

namespace NBi.Core.Query
{
    public class QueryOleDbEngine:IQueryExecutor, IQueryPerformance, IQueryParser, IQueryEnginable
    {
        protected readonly OleDbCommand _command;


        public QueryOleDbEngine(OleDbCommand cmd)
        {
            _command = cmd;
        }

        public virtual ParserResult Parse()
        {
            ParserResult res=null;
            
            using(var conn = new OleDbConnection(_command.Connection.ConnectionString))
            {
                var fullSql = string.Format(@"SET FMTONLY ON {0} SET FMTONLY OFF", _command.CommandText);
                
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

        public virtual void CleanCache()
        {
            using (var conn = new OleDbConnection(_command.Connection.ConnectionString))
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

        public virtual PerformanceResult CheckPerformance()
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

        public virtual DataSet Execute()
        {
            int i;
            return Execute(out i);
        }

        public virtual DataSet Execute(out int elapsedSec)
        {
            // Open the connection
            using (var connection = new OleDbConnection())
            {
                var connectionString = _command.Connection.ConnectionString;
                try
                    { connection.ConnectionString = connectionString; }
                catch (ArgumentException ex)
                { throw new ConnectionException(ex, connectionString); }

                try
                    {connection.Open();}
                catch (OleDbException ex)
                { throw new ConnectionException(ex, connectionString); }

                // capture time before execution
                long ticksBefore = DateTime.Now.Ticks;
                var adapter = new OleDbDataAdapter(_command.CommandText, connection);
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
