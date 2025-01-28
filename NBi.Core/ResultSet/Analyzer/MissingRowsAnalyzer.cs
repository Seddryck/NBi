using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer;

class MissingRowsAnalyzer : BaseRowsAnalyzer
{
    protected override string Sentence
    {
        get => "Missing rows";
    }

    protected override List<RowHelper> ExtractRows(Dictionary<KeyCollection, RowHelper> x, Dictionary<KeyCollection, RowHelper> y)
    {
        var keys = y.Keys.Except(x.Keys);
        var rows = new List<RowHelper>(keys.Count());
        foreach (var i in keys)
            rows.Add(y[i]);
        return rows;
    }
}
