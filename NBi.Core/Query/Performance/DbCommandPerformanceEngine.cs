using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.Query.Performance
{
    internal class DbCommandPerformanceEngine : IPerformanceEngine
    {
        protected readonly DbCommandExecutionEngine engine;
        private bool isTimeout = false;

        public DbCommandPerformanceEngine(DbCommandExecutionEngine engine)
        {
            this.engine = engine;
        }

        public PerformanceResult Execute()
        {
            engine.OnTimeout = (ex, cmd) => { isTimeout = true; };
            engine.Execute();
            if (isTimeout)
                return PerformanceResult.Timeout(engine.CommandTimeout);
            else
                return new PerformanceResult(engine.Elapsed);
        }

        public PerformanceResult Execute(TimeSpan timeout)
        {
            engine.CommandTimeout = timeout;
            return this.Execute();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual void CleanCache()
        {
            using var conn = engine.NewConnection();
            var clearSql = new string[] { "dbcc freeproccache", "dbcc dropcleanbuffers" };
            conn.ConnectionString = engine.ConnectionString;
            conn.Open();

            foreach (var sql in clearSql)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
