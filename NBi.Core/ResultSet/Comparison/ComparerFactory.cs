using NBi.Core.ResultSet.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Comparison
{
    public class ComparerFactory
    {
        public IComparer Instantiate(ISettingsResultSet settings, ComparerKind kind)
        {
            if (settings is SettingsSingleRowResultSet)
                return new SingleRowComparer(settings as SettingsSingleRowResultSet);
            else
            {
                var factory = new AnalyzersFactory();
                var analyzers = factory.Instantiate(kind);

                if (settings is SettingsIndexResultSet)
                    return new IndexComparer(analyzers, settings as SettingsIndexResultSet);

                else if (settings is SettingsNameResultSet)
                    return new NameComparer(analyzers, settings as SettingsNameResultSet);
            }
            throw new ArgumentOutOfRangeException(nameof(settings));
        }
    }
}
