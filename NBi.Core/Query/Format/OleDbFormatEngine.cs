using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Core.Query.Execution;
using System.Data.OleDb;

namespace NBi.Core.Query.Format;

internal class OleDbFormatEngine : OleDbExecutionEngine, IFormatEngine
{
    protected internal OleDbFormatEngine(OleDbConnection connection, OleDbCommand command)
        : base(connection, command)
    { }

    public IEnumerable<string> ExecuteFormat()
    {
        return base.ExecuteList<string>();
    }
}
