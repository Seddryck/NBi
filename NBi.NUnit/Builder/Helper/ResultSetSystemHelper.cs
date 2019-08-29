using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Alteration.Renaming;
using NBi.Core.ResultSet.Alteration.Summarization;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Conversion;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Linq;
using System.Collections.Generic;
using NBi.Core.ResultSet.Alteration.Reshaping;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.Alteration.Summarization;
using NBi.Xml.Items.Alteration.Extension;
using NBi.Xml.Items.Alteration.Reshaping;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Renaming;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetSystemHelper
    {
        private readonly ServiceLocator serviceLocator;
        private SettingsXml.DefaultScope scope = SettingsXml.DefaultScope.Everywhere;
        private readonly IDictionary<string, ITestVariable> variables;

        public ResultSetSystemHelper(ServiceLocator serviceLocator, SettingsXml.DefaultScope scope, IDictionary<string, ITestVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.scope = scope;
            this.variables = variables;
        }

        public IResultSetResolver InstantiateResolver(ResultSetSystemXml resultSetXml)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(serviceLocator);
            argsBuilder.Setup(resultSetXml, resultSetXml.Settings, scope, variables);
            argsBuilder.Build();

            var factory = serviceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());
            return resolver;
        }

        public IEnumerable<Alter> InstantiateAlterations(ResultSetSystemXml resultSetXml)
        {
            if ((resultSetXml.Alterations?.Count ?? 0) == 0)
                yield break;

            foreach (var alterationXml in resultSetXml.Alterations)
            {
                switch(alterationXml)
                {
                    case FilterXml x: yield return InstantiateFilter(x); break;
                    case ConvertXml x: yield return InstantiateConvert(x); break;
                    case TransformXml x: yield return InstantiateTransform(x); break;
                    case RenamingXml x: yield return InstantiateRename(x); break;
                    case SummarizeXml x: yield return InstantiateSummarize(x); break;
                    case ExtendXml x: yield return InstantiateExtend(x); break;
                    case UnstackXml x: yield return InstantiateUnstack(x); break;
                    default: throw new ArgumentException();
                }
            }
        }

        private Alter InstantiateFilter(FilterXml filterXml)
        {
            var factory = new ResultSetFilterFactory(variables);

            if (filterXml.Ranking == null)
            {
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression != null)
                    expressions.Add(filterXml.Expression);

                if (filterXml.Predication != null)
                {
                    var helper = new PredicateArgsBuilder(serviceLocator, variables);
                    var args = helper.Execute(filterXml.Predication.ColumnType, filterXml.Predication.Predicate);

                    return factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , new PredicationArgs(filterXml.Predication.Operand, args)
                                ).Apply;
                }
                if (filterXml.Combination != null)
                {
                    var helper = new PredicateArgsBuilder(serviceLocator, variables);
                    var predicationArgs = new List<PredicationArgs>();
                    foreach (var predication in filterXml.Combination.Predications)
                    {
                        var args = helper.Execute(predication.ColumnType, predication.Predicate);
                        predicationArgs.Add(new PredicationArgs(predication.Operand, args));
                    }

                    return factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , filterXml.Combination.Operator
                                    , predicationArgs
                                ).Apply;
                }
                throw new ArgumentException();
            }
            else
            {
                return factory.Instantiate(
                    filterXml.Ranking,
                    filterXml.Ranking?.GroupBy?.Columns
                    ).Apply;
            }
        }

        private Alter InstantiateConvert(ConvertXml convertXml)
        {
            var factory = new ConverterFactory();
            var converter = factory.Instantiate(convertXml.Converter.From, convertXml.Converter.To, convertXml.Converter.DefaultValue, convertXml.Converter.Culture);
            var engine = new ConverterEngine(convertXml.Column, converter);
            return engine.Execute;
        }

        private Alter InstantiateRename(RenamingXml renameXml)
        {
            var helper = new ScalarHelper(serviceLocator, variables);
            var newName = helper.InstantiateResolver<string>(renameXml.NewName);

            var factory = new RenamingFactory();
            var renamer = factory.Instantiate(new NewNameRenamingArgs(renameXml.Identifier, newName));
            return renamer.Execute;
        }

        private Alter InstantiateTransform(TransformXml transformXml)
        {
            var identifierFactory = new ColumnIdentifierFactory();
            var provider = new TransformationProvider();
            provider.Add(transformXml.Identifier, transformXml);
            return provider.Transform;
        }

        private Alter InstantiateSummarize(SummarizeXml summarizeXml)
        {
            var factory = new SummarizationFactory();
            var aggregations = new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(
                            summarizeXml.Aggregation.Identifier,
                            summarizeXml.Aggregation.Function,
                            summarizeXml.Aggregation.ColumnType
                        )
                    };
            var groupBys = summarizeXml.GroupBy?.Columns?.Cast<IColumnDefinitionLight>() ?? new List<IColumnDefinitionLight>();

            var summarizer = factory.Instantiate(new SummarizeArgs(aggregations, groupBys));
            return summarizer.Execute;
        }

        private Alter InstantiateExtend(ExtendXml extendXml)
        {
            var factory = new ExtensionFactory();
            var extender = factory.Instantiate(new ExtendArgs
                (
                    extendXml.Identifier
                    , extendXml.Script?.Code ?? throw new ArgumentException("Script cannot be empty or null")
                ));
            return extender.Execute;
        }

        private Alter InstantiateUnstack(UnstackXml unstackXml)
        {
            var factory = new ReshapingFactory();
            var header = unstackXml.Header.Column.Identifier;
            var groupBys = unstackXml.GroupBy?.Columns?.Cast<IColumnDefinitionLight>() ?? new List<IColumnDefinitionLight>();

            var reshaper = factory.Instantiate(new UnstackArgs(header, groupBys));
            return reshaper.Execute;
        }
    }
}