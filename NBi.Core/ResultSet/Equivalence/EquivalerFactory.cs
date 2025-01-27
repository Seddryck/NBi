using NBi.Core.ResultSet.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Equivalence;

public class EquivalerFactory
{
    public IEquivaler Instantiate(ISettingsResultSet settings, EquivalenceKind kind)
    {
        return settings switch
        {
            ISettingsSingleRowResultSet x => InstantiateSingleRow(x),
            ISettingsResultSet x => InstantiateMultipleRows(x, kind),
            _ => throw new ArgumentException(),
        };
    }

    public IEquivaler InstantiateSingleRow(ISettingsSingleRowResultSet settings)
    {
        return settings switch
        {
            SettingsSingleRowOrdinalResultSet x => new SingleRowOrdinalEquivaler(x),
            SettingsSingleRowNameResultSet x => new SingleRowNameEquivaler(x),
            _ => throw new ArgumentException(),
        };
    }

    public IEquivaler InstantiateMultipleRows(ISettingsResultSet settings, EquivalenceKind kind)
    {
        var factory = new AnalyzersFactory();
        var analyzers = factory.Instantiate(kind);

        return settings switch
        {
            SettingsOrdinalResultSet x => new OrdinalEquivaler(analyzers, x),
            SettingsNameResultSet x => new NameEquivaler(analyzers, x),
            _ => throw new ArgumentException(),
        };
    }
}
