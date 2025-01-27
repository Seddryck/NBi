using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;

public class SkipAlterationStrategy : IMissingColumnStrategy
{
    public void Execute(string originalColumnName, IResultSet dataTable)
    {
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Alteration renaming for column '{originalColumnName}' was skipped because no column has this name in the result-set.");
    }
}
