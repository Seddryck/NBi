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
using alt = NBi.Xml.Items.Alteration;
using NBi.Xml.Items.Alteration.Summarization;
using NBi.Xml.Items.Alteration.Extension;
using NBi.Xml.Items.Alteration.Reshaping;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Renaming;
using NBi.Xml.Items.Alteration.Projection;
using NBi.Core.ResultSet.Alteration.Projection;
using NBi.Xml.Items.Alteration.Lookup;
using NBi.Core.ResultSet.Alteration.Lookup;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing;
using NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;
using NBi.Core.ResultSet.Filtering;
using NBi.Core.Calculation.Grouping;
using NBi.Xml.Items.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.Calculation.Grouping.CaseBased;
using NBi.Core.Calculation.Predication;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetSystemHelper
    {
        protected ServiceLocator ServiceLocator { get; }
        protected SettingsXml.DefaultScope Scope { get; } = SettingsXml.DefaultScope.Everywhere;
        protected IDictionary<string, ITestVariable> Variables { get; }

        public ResultSetSystemHelper(ServiceLocator serviceLocator, SettingsXml.DefaultScope scope, IDictionary<string, ITestVariable> variables)
            => (ServiceLocator, Scope, Variables) = (serviceLocator, scope, variables);

        public IResultSetResolver InstantiateResolver(ResultSetSystemXml resultSetXml)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(ServiceLocator);
            argsBuilder.Setup(resultSetXml, resultSetXml.Settings, Scope, Variables);
            argsBuilder.Build();

            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());
            return resolver;
        }

        public IEnumerable<Alter> InstantiateAlterations(ResultSetSystemXml resultSetXml)
        {
            if ((resultSetXml.Alterations?.Count ?? 0) == 0)
                yield break;

            foreach (var alterationXml in resultSetXml.Alterations)
            {
                switch (alterationXml)
                {
                    case FilterXml x: yield return InstantiateFilter(x); break;
                    case ConvertXml x: yield return InstantiateConvert(x); break;
                    case TransformXml x: yield return InstantiateTransform(x); break;
                    case RenamingXml x: yield return InstantiateRename(x); break;
                    case SummarizeXml x: yield return InstantiateSummarize(x); break;
                    case ExtendXml x: yield return InstantiateExtend(x); break;
                    case UnstackXml x: yield return InstantiateUnstack(x); break;
                    case ProjectAwayXml x: yield return InstantiateProjectAway(x); break;
                    case ProjectXml x: yield return InstantiateProject(x); break;
                    case LookupReplaceXml x: yield return InstantiateLookupReplace(x, resultSetXml.Settings); break;
                    default: throw new ArgumentException();
                }
            }
        }

        private Alter InstantiateFilter(FilterXml filterXml)
        {
            var context = new Context(Variables);
            var factory = new ResultSetFilterFactory(ServiceLocator);

            if (filterXml.Ranking == null)
            {
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression != null)
                    expressions.Add(filterXml.Expression);

                if (filterXml.Predication != null)
                {
                    var helper = new PredicateArgsBuilder(ServiceLocator, context);
                    var args = helper.Execute(filterXml.Predication.ColumnType, filterXml.Predication.Predicate);

                    return factory.Instantiate
                                (
                                    new PredicationArgs(filterXml.Predication.Operand, args)
                                    , context
                                ).Apply;
                }
                if (filterXml.Combination != null)
                {
                    var helper = new PredicateArgsBuilder(ServiceLocator, context);
                    var predicationArgs = new List<PredicationArgs>();
                    foreach (var predication in filterXml.Combination.Predications)
                    {
                        var args = helper.Execute(predication.ColumnType, predication.Predicate);
                        predicationArgs.Add(new PredicationArgs(predication.Operand, args));
                    }

                    return factory.Instantiate
                                (
                                    filterXml.Combination.Operator
                                    , predicationArgs
                                    , context
                                ).Apply;
                }
                throw new ArgumentException();
            }
            else
            {
                var groupByArgs = BuildGroupByArgs(filterXml.Ranking.GroupBy, context);
                var groupByFactory = new GroupByFactory();
                var groupBy = groupByFactory.Instantiate(groupByArgs);

                var rankingGroupByArgs = new RankingGroupByArgs(groupBy, filterXml.Ranking.Option, filterXml.Ranking.Count, filterXml.Ranking.Operand, filterXml.Ranking.Type);
                return factory.Instantiate(rankingGroupByArgs, context).Apply;
            }
        }

        private IGroupByArgs BuildGroupByArgs(GroupByXml xml, Context context)
        {
            if (xml == null)
                return new NoneGroupByArgs();
            if ((xml?.Columns?.Count ?? 0) > 0)
                return new ColumnGroupByArgs(xml.Columns, context);
            if ((xml?.Cases?.Count ?? 0) > 0)
            {
                var builder = new PredicateArgsBuilder(ServiceLocator, context);
                var predications = new List<IPredication>();
                foreach (var caseXml in xml.Cases)
                {
                    if (caseXml.Predication is SinglePredicationXml)
                    {
                        var predicationXml = (caseXml.Predication) as SinglePredicationXml;
                        var args = builder.Execute(predicationXml.ColumnType, predicationXml.Predicate);
                        var predicate = new PredicateFactory().Instantiate(args);
                        var predicationFactory = new PredicationFactory();
                        predications.Add(predicationFactory.Instantiate(predicate, predicationXml.Operand));

                    }
                }

                return new CaseGroupByArgs(predications, context);
            }
            throw new ArgumentOutOfRangeException();
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
            var helper = new ScalarHelper(ServiceLocator, new Context(Variables));
            var newName = helper.InstantiateResolver<string>(renameXml.NewName);

            IMissingColumnStrategy strategy = new FailureMissingColumnStrategy();
            switch (renameXml.Missing.Behavior)
            {
                case alt.Renaming.MissingColumnBehavior.Skip:
                    strategy = new SkipAlterationStrategy();
                    break;
                default:
                    strategy = new FailureMissingColumnStrategy();
                    break;
            }

            var factory = new RenamingFactory();
            var renamer = factory.Instantiate(new NewNameRenamingArgs(renameXml.Identifier, newName, strategy));
            return renamer.Execute;
        }

        private Alter InstantiateTransform(TransformXml transformXml)
        {
            var identifierFactory = new ColumnIdentifierFactory();
            var provider = new TransformationProvider(new ServiceLocator(), new Context(Variables));
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
            var factory = new ExtensionFactory(ServiceLocator);
            var extender = factory.Instantiate(new ExtendArgs
                (
                    extendXml.Identifier
                    , extendXml.Script?.Code ?? throw new ArgumentException("Script cannot be empty or null")
                    , extendXml.Script.Language
                ));
            return extender.Execute;
        }

        private Alter InstantiateUnstack(UnstackXml unstackXml)
        {
            var factory = new ReshapingFactory();
            var header = unstackXml.Header.Column.Identifier;
            var groupBys = unstackXml.GroupBy?.Columns?.Cast<IColumnDefinitionLight>() ?? new List<IColumnDefinitionLight>();
            var values = unstackXml.Header.EnforcedValues.Select(x => new ColumnNameIdentifier(x));

            var reshaper = factory.Instantiate(new UnstackArgs(header, groupBys, values));
            return reshaper.Execute;
        }

        private Alter InstantiateProject(ProjectXml projectXml)
        {
            var factory = new ProjectionFactory();
            var project = factory.Instantiate(new ProjectArgs(projectXml.Columns.Select(x => x.Identifier)));
            return project.Execute;
        }

        private Alter InstantiateProjectAway(ProjectAwayXml projectXml)
        {
            var factory = new ProjectionFactory();
            var project = factory.Instantiate(new ProjectAwayArgs(projectXml.Columns.Select(x => x.Identifier)));
            return project.Execute;
        }

        private Alter InstantiateLookupReplace(LookupReplaceXml lookupReplaceXml, SettingsXml settingsXml)
        {
            var factory = new LookupFactory();

            var innerService = new ResultSetServiceBuilder();
            lookupReplaceXml.ResultSet.Settings = settingsXml;
            innerService.Setup(InstantiateResolver(lookupReplaceXml.ResultSet));
            innerService.Setup(InstantiateAlterations(lookupReplaceXml.ResultSet));

            IMissingStrategy strategy = new FailureMissingStrategy();
            switch (lookupReplaceXml.Missing.Behavior)
            {
                case alt.Lookup.Behavior.OriginalValue:
                    strategy = new OriginalValueMissingStrategy();
                    break;
                case alt.Lookup.Behavior.DefaultValue:
                    strategy = new DefaultValueMissingStrategy(lookupReplaceXml.Missing.DefaultValue);
                    break;
                case alt.Lookup.Behavior.DiscardRow:
                    strategy = new DiscardRowMissingStrategy();
                    break;
                default:
                    strategy = new FailureMissingStrategy();
                    break;
            }

            var lookup = factory.Instantiate(
                    new LookupReplaceArgs(
                        innerService.GetService(),
                        BuildMappings(lookupReplaceXml.Join).ElementAt(0),
                        lookupReplaceXml.Replacement.Identifier,
                        strategy
                ));

            return lookup.Execute;
        }

        private IEnumerable<ColumnMapping> BuildMappings(JoinXml joinXml)
        {
            var factory = new ColumnIdentifierFactory();

            return joinXml?.Mappings.Select(mapping => new ColumnMapping(
                        factory.Instantiate(mapping.Candidate)
                        , factory.Instantiate(mapping.Reference)
                        , mapping.Type))
                .Union(
                    joinXml?.Usings.Select(@using => new ColumnMapping(
                        factory.Instantiate(@using.Column)
                        , @using.Type)
                    ));
        }
    }
}