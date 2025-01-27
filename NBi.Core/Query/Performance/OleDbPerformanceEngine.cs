using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Performance;

[SupportedCommandType(typeof(OleDbCommand))]
internal class OleDbPerformanceEngine : DbCommandPerformanceEngine
{
    
    protected internal OleDbPerformanceEngine(OleDbConnection connection, OleDbCommand command)
        : base(new OleDbExecutionEngine(connection, command))
    { }
}
