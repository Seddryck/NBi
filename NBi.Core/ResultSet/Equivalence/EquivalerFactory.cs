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
            switch (settings)
            {
                case ISettingsSingleRowResultSet x: return InstantiateSingleRow(x);
                case ISettingsResultSet x: return InstantiateMultipleRows(x, kind);
                default: throw new ArgumentException();
            }
        }

        public IEquivaler InstantiateSingleRow(ISettingsSingleRowResultSet settings)
        {
            switch (settings)
            {
                case SettingsSingleRowOrdinalResultSet x: return new SingleRowOrdinalEquivaler(x);
                case SettingsSingleRowNameResultSet x: return new SingleRowNameEquivaler(x);
                default: throw new ArgumentException();
            }
        }

        public IEquivaler InstantiateMultipleRows(ISettingsResultSet settings, EquivalenceKind kind)
        {
            var factory = new AnalyzersFactory();
            var analyzers = factory.Instantiate(kind);

            switch (settings)
            {
                case SettingsOrdinalResultSet x: return new OrdinalEquivaler(analyzers, x);
                case SettingsNameResultSet x: return new NameEquivaler(analyzers, x);
                default: throw new ArgumentException();
            }
        }
    }
}
