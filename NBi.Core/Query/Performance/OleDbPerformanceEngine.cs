using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace NBi.Core.Query.Performance
{
    internal class OleDbPerformanceEngine : DbCommandPerformanceEngine
    {
        protected internal OleDbPerformanceEngine(OleDbCommand command)
            : base(new OleDbExecutionEngine(command))
        { }
    }
}
