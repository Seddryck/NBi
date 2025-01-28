using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Core.Query.Execution;
using System.Data.Odbc;

namespace NBi.Core.Query.Format;

internal class OdbcFormatEngine : OdbcExecutionEngine, IFormatEngine
{
    protected internal OdbcFormatEngine(OdbcConnection connection, OdbcCommand command)
        : base(connection, command)
    { }

    public IEnumerable<string> ExecuteFormat()
    {
        return base.ExecuteList<string>();
    }
}
