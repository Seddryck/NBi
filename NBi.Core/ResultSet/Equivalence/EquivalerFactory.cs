using NBi.Core.ResultSet.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Equivalence
{
    public class EquivalerFactory
    {
        public IEquivaler Instantiate(ISettingsResultSet settings, EquivalenceKind kind)
        {
            if (settings is SettingsSingleRowResultSet)
                return new SingleRowEquivaler(settings as SettingsSingleRowResultSet);
            else
            {
                var factory = new AnalyzersFactory();
                var analyzers = factory.Instantiate(kind);

                if (settings is SettingsOrdinalResultSet)
                    return new OrdinalEquivaler(analyzers, settings as SettingsOrdinalResultSet);

                else if (settings is SettingsNameResultSet)
                    return new NameEquivaler(analyzers, settings as SettingsNameResultSet);
            }
            throw new ArgumentOutOfRangeException(nameof(settings));
        }
    }
}
