using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer;

abstract class BaseRowsAnalyzer : IRowsAnalyzer
{
    protected abstract string Sentence { get; }

    public List<RowHelper> Retrieve(Dictionary<KeyCollection,RowHelper> x, Dictionary<KeyCollection, RowHelper> y)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var rows = ExtractRows(x, y);
        stopWatch.Stop();
        Trace.WriteLineIf(
            Extensibility.NBiTraceSwitch.TraceInfo, 
            $"{Sentence}: {rows.Count}  [{stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]"
            );

        return rows;
    }

    protected abstract List<RowHelper>  ExtractRows(Dictionary<KeyCollection, RowHelper> x, Dictionary<KeyCollection, RowHelper> y);
    
}
