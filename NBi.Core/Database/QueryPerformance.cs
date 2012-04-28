using System;
using System.Data;
using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class QueryPerformance : IQueryPerformance
    {
        protected bool _cleanCache;
        protected int _maxTimeMilliSeconds;

        public QueryPerformance(int maxTimeMilliSeconds, bool cleanCache)
        {
            _maxTimeMilliSeconds= maxTimeMilliSeconds;
            _cleanCache = cleanCache;
        }

        public Result Validate(IDbCommand cmd)
        {
            DateTime tsStart, tsStop; 

            if(_cleanCache)
            {
                using (var conn = new SqlConnection(cmd.Connection.ConnectionString))
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

            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();

            tsStart = DateTime.Now;
            cmd.ExecuteNonQuery();
            tsStop = DateTime.Now;

            if (cmd.Connection.State == ConnectionState.Open)
                cmd.Connection.Close();

            double ms = tsStop.Subtract(tsStart).TotalMilliseconds;

            if (ms > _maxTimeMilliSeconds)
                return Result.Failed(String.Format("Maximum time specified was {0} but execution had token {1}", _maxTimeMilliSeconds, ms));
            else
                return Result.Success();
        }
    }
}