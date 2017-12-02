using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;

namespace NBi.Core.Query.Performance
{
    internal class OdbcPerformanceEngine : DbCommandPerformanceEngine
    {
        protected internal OdbcPerformanceEngine(OdbcCommand command)
            : base(new OdbcExecutionEngine(command))
        { }
    }
}