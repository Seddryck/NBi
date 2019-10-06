using NBi.Core;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class PredicateArgsBuilder
    {
        private bool IsSetup { get; } = false;
        private PredicateArgs Args { get; } = null;

        private SettingsXml Settings { get; } = SettingsXml.Empty;
        private IDictionary<string, ITestVariable> Variables { get; } = new Dictionary<string, ITestVariable>();

        private ServiceLocator ServiceLocator { get; }

        public PredicateArgsBuilder(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables) 
            => (ServiceLocator, Variables) = (serviceLocator, variables);


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
            => new ScalarHelper(ServiceLocator, new Context(Variables)).InstantiateResolver(columnType, xml.Reference);

        private IResolver BuildSequenceReference(ColumnType columnType, SequenceReferencePredicateXml xml)
            => new ListSequenceResolver<string>(xml.References);

        private SecondOperandPredicateArgs BuildSecondOperandPredicateArgs(ColumnType columnType, SecondOperandPredicateXml xml)
            => new SecondOperandPredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
                Reference = BuildScalarReference(columnType, xml),
                SecondOperand = xml.SecondOperand
            };

        private CultureSensitivePredicateArgs BuildCultureSensitivePredicateArgs(ColumnType columnType, ICultureSensitiveTextPredicateXml xml)
            => new CultureSensitivePredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
                Culture = xml.Culture
            };

        private CaseSensitivePredicateArgs BuildCaseSensitivePredicateArgs(ColumnType columnType, ICaseSensitiveTextPredicateXml xml)
            => new CaseSensitivePredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
                StringComparison = xml.IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture,
                Reference = xml is SequenceReferencePredicateXml 
                ? BuildSequenceReference(columnType, xml as SequenceReferencePredicateXml) 
                : BuildScalarReference(columnType, xml as ScalarReferencePredicateXml)
            };

        private ReferencePredicateArgs BuildReferencePredicateArgs(ColumnType columnType, ReferencePredicateXml xml)
            => new CaseSensitivePredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
                Reference = xml is SequenceReferencePredicateXml
                ? BuildSequenceReference(columnType, xml as SequenceReferencePredicateXml)
                : BuildScalarReference(xml is WithinRangeXml ? ColumnType.Text : columnType, xml as ScalarReferencePredicateXml)
            };

        private PredicateArgs BuildPredicateArgs(ColumnType columnType, PredicateXml xml)
            => new CaseSensitivePredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = xml.ComparerType,
                Not = xml.Not,
            };
    }
}
