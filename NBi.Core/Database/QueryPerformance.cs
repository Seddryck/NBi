using System;
using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class QueryPerformance : IQueryPerformance
    {
        protected string _connectionString;
        protected int _maxTimeMilliSeconds;

        public QueryPerformance(string connectionString, int maxTimeMilliSeconds)
        {
            _connectionString = connectionString;
            _maxTimeMilliSeconds= maxTimeMilliSeconds;
        }

        public Result Validate(string sqlQuery)
        {
            DateTime tsStart, tsStop; 

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var clearSql = new string[] { "dbcc freeproccache", "dbcc dropcleanbuffers" };
                
                conn.Open();

                foreach (var sql in clearSql)
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    tsStart = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    tsStop = DateTime.Now;
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            double ms = tsStop.Subtract(tsStart).TotalMilliseconds;

            if (ms > _maxTimeMilliSeconds)
                return Result.Failed(String.Format("Maximum time specified was {0} but execution had token {1}", _maxTimeMilliSeconds, ms));
            else
                return Result.Success();
        }
    }
}