using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping.ColumnBased;

public abstract class ColumnGrouping : IGroupBy
{
    protected ISettingsResultSet Settings { get; }
    protected Context Context { get; }

    protected ColumnGrouping(ISettingsResultSet settings, Context context)
        => (Settings, Context) = (settings, context);

    public IDictionary<KeyCollection, IResultSet> Execute(IResultSet resultSet)
    {
        var stopWatch = new Stopwatch();
        var dico = new Dictionary<KeyCollection, IResultSet>(new KeyCollectionEqualityComparer());
        var keyComparer = BuildDataRowsKeyComparer(resultSet);

        stopWatch.Start();
        foreach (var row in resultSet.Rows)
        {
            Context.Switch(row);
            var key = keyComparer.GetKeys(row);
            if (!dico.ContainsKey(key))
                dico.Add(key, resultSet.Clone());
            dico[key].AddRow(row);
        }
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building rows' groups: {dico.Count} [{stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s})]");
        return dico;
    }

    protected abstract DataRowKeysComparer BuildDataRowsKeyComparer(IResultSet x);
}
