using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.Query.Performance
{
    [SupportedCommandType(typeof(SqlCommand))]
    internal class SqlPerformanceEngine : DbCommandPerformanceEngine
    {
        protected internal SqlPerformanceEngine(SqlConnection connection, SqlCommand command)
            : base(new SqlExecutionEngine(connection, command))
        { }
    }
}
