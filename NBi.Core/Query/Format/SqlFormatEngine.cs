using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Core.Query.Execution;
using Microsoft.Data.SqlClient;

namespace NBi.Core.Query.Format;

internal class SqlFormatEngine : SqlExecutionEngine, IFormatEngine
{
    protected internal SqlFormatEngine(SqlConnection connection, SqlCommand command)
        : base(connection, command)
    { }

    public IEnumerable<string> ExecuteFormat()
    {
        return base.ExecuteList<string>();
    }
}
