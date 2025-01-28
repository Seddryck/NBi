using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Performance;

[SupportedCommandType(typeof(OdbcCommand))]
internal class OdbcPerformanceEngine : DbCommandPerformanceEngine
{
    protected internal OdbcPerformanceEngine(OdbcConnection connection, OdbcCommand command)
        : base(new OdbcExecutionEngine(connection, command))
    { }
}