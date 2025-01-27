using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer;

class UnexpectedRowsAnalyzer : BaseRowsAnalyzer
{
    protected override string Sentence
    {
        get => "Unexpected rows";
    }

    protected override List<RowHelper> ExtractRows(Dictionary<KeyCollection, RowHelper> x, Dictionary<KeyCollection, RowHelper> y)
    {
        var keys = x.Keys.Except(y.Keys);
        var rows = new List<RowHelper>(keys.Count());
        foreach (var i in keys)
            rows.Add(x[i]);
        return rows;
    }
}
