using NBi.Core;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Model.Xml.Constraints.Comparer;
using NBi.Extensibility.Resolving;
using NBi.Model.Xml.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class PredicateArgsBuilder
    {
        private SettingsXml Settings { get; } = SettingsXml.Empty;
        private Context Context { get; }

        private ServiceLocator ServiceLocator { get; }

        public PredicateArgsBuilder(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public PredicateArgs Execute(ColumnType columnType, PredicateXml xml)
        {
            switch (xml)
            {
                case SecondOperandPredicateXml x: return BuildSecondOperandPredicateArgs(columnType, x);
                case ICaseSensitiveTextPredicateXml x: return BuildCaseSensitivePredicateArgs(columnType, x);
                case ICultureSensitiveTextPredicateXml x: return BuildCultureSensitivePredicateArgs(columnType, x);
                case ReferencePredicateXml x: return BuildReferencePredicateArgs(columnType, x);
                case PredicateXml x: return BuildPredicateArgs(columnType, x);
            }
            throw new NotImplementedException();
        }

        private IResolver BuildScalarReference(ColumnType columnType, ScalarReferencePredicateXml xml)
            => new ScalarHelper(ServiceLocator, Context).InstantiateResolver(columnType, xml.Reference);

        private IResolver BuildSequenceReference(ColumnType columnType, SequenceReferencePredicateXml xml)
            => new ListSequenceResolver<string>(xml.References);

        private SecondOperandPredicateArgs BuildSecondOperandPredicateArgs(ColumnType columnType, SecondOperandPredicateXml xml)
            => new SecondOperandPredicateArgs(BuildScalarReference(columnType, xml), xml.SecondOperand)
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not
            };

        private CultureSensitivePredicateArgs BuildCultureSensitivePredicateArgs(ColumnType columnType, ICultureSensitiveTextPredicateXml xml)
            => new CultureSensitivePredicateArgs(xml.Culture)
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not
            };

        private CaseSensitivePredicateArgs BuildCaseSensitivePredicateArgs(ColumnType columnType, ICaseSensitiveTextPredicateXml xml)
            => new CaseSensitivePredicateArgs(
                xml is SequenceReferencePredicateXml sequence
                    ? BuildSequenceReference(columnType, sequence)
                    : BuildScalarReference(columnType, (ScalarReferencePredicateXml)xml),
                xml.IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
            };

        private ReferencePredicateArgs BuildReferencePredicateArgs(ColumnType columnType, ReferencePredicateXml xml)
            => new ReferencePredicateArgs(
                xml is SequenceReferencePredicateXml sequence
                    ? BuildSequenceReference(columnType, sequence)
                    : BuildScalarReference(xml is WithinRangeXml ? ColumnType.Text : columnType, (ScalarReferencePredicateXml)xml))
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
            };

        private PredicateArgs BuildPredicateArgs(ColumnType columnType, PredicateXml xml)
            => new PredicateArgs
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
            };
    }
}
