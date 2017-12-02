using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.Query.Performance
{
    internal class SqlPerformanceEngine : DbCommandPerformanceEngine
    {
        protected internal SqlPerformanceEngine(SqlCommand command)
            : base(new SqlExecutionEngine(command))
        { }
    }
}
