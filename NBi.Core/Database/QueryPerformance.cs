using System;
using System.Data.SqlClient;

namespace NBi.Core.Database
{
    public class QueryPerformance
    {
        protected string _connectionString;
        protected string _sqlQuery;

        public QueryPerformance(string connectionString, string sqlQuery)
        {
            _connectionString = connectionString;
            _sqlQuery = sqlQuery;
        }

        public Result Validate(int maxTimeMilliSeconds)
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
                
                using (SqlCommand cmd = new SqlCommand(_sqlQuery, conn))
                {
                    tsStart = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    tsStop = DateTime.Now;
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            double ms = tsStop.Subtract(tsStart).TotalMilliseconds;

            if (ms > maxTimeMilliSeconds)
                return Result.Failed(String.Format("Maximum time specified was {0} but execution had token {1}", maxTimeMilliSeconds, ms));
            else
                return Result.Success();
        }
    }
}