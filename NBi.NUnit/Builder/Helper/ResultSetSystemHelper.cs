using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Alteration.Renaming;
using NBi.Core.ResultSet.Alteration.Summarization;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.Scalar.Conversion;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NBi.Extensibility.Resolving;
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
using NBi.Xml.Items.Alteration.Merging;
using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Xml.Items.Alteration.Duplication;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.ResultSet.Resolver;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetSystemHelper
    {
        protected ServiceLocator ServiceLocator { get; }
        protected SettingsXml.DefaultScope Scope { get; } = SettingsXml.DefaultScope.Everywhere;
        protected IDictionary<string, IVariable> Variables { get; }

        public ResultSetSystemHelper(ServiceLocator serviceLocator, SettingsXml.DefaultScope scope, IDictionary<string, IVariable> variables)
            => (ServiceLocator, Scope, Variables) = (serviceLocator, scope, variables);

        public IResultSetResolver InstantiateResolver(ResultSetSystemXml resultSetXml)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(ServiceLocator);
            argsBuilder.Setup(resultSetXml, resultSetXml.Settings, Scope, Variables);
            argsBuilder.Build();

            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());

            if ((resultSetXml.Alterations?.Count ?? 0) == 0)
                return resolver;

            var alterations = InstantiateAlterations(resultSetXml);
            var alteredArgs = new AlterationResultSetResolverArgs(resolver, alterations);
            return factory.Instantiate(alteredArgs);
        }

        public IEnumerable<IAlteration> InstantiateAlterations(ResultSetSystemXml resultSetXml)
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
                    case MergeXml x: yield return InstantiateMerging(x, resultSetXml.Settings); break;
                    case DuplicateXml x: yield return InstantiateDuplicate(x); break;
                    default: throw new ArgumentException();
                }
            }
        }

        private IAlteration InstantiateFilter(FilterXml filterXml)
        {
            var context = new Context(Variables);
            var factory = new ResultSetFilterFactory(ServiceLocator);

            if (filterXml.Ranking == null && filterXml.Uniqueness == null)
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
                                );
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
                                );
                }
                throw new ArgumentException();
            }
            else if (filterXml.Ranking != null)
            {
                var groupByArgs = BuildGroupByArgs(filterXml.Ranking.GroupBy, context);
                var groupByFactory = new GroupByFactory();
                var groupBy = groupByFactory.Instantiate(groupByArgs);

                var rankingGroupByArgs = new RankingGroupByArgs(groupBy, filterXml.Ranking.Option, filterXml.Ranking.Count, filterXml.Ranking.Operand, filterXml.Ranking.Type);
                return factory.Instantiate(rankingGroupByArgs, context);
            }

            else if (filterXml.Uniqueness != null)
            {
                var groupByArgs = BuildGroupByArgs(filterXml.Uniqueness.GroupBy, context);
                var groupByFactory = new GroupByFactory();
                var groupBy = groupByFactory.Instantiate(groupByArgs);

                var uniquenessArgs = new UniquenessArgs(groupBy);
                return factory.Instantiate(uniquenessArgs, context);
            }

            throw new ArgumentOutOfRangeException();
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

        private IAlteration InstantiateConvert(ConvertXml convertXml)
        {
            var factory = new ConverterFactory();
            var converter = factory.Instantiate(convertXml.Converter.From, convertXml.Converter.To, convertXml.Converter.DefaultValue, convertXml.Converter.Culture);
            var convertion = new ConverterEngine(convertXml.Column, converter);
            return convertion;
        }

        private IAlteration InstantiateRename(RenamingXml renameXml)
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
            var renaming = factory.Instantiate(new NewNameRenamingArgs(renameXml.Identifier, newName, strategy));
            return renaming;
        }

        private IAlteration InstantiateMerging(MergeXml mergeXml, SettingsXml settingsXml)
        {
            mergeXml.ResultSet.Settings = settingsXml;
            var innerResolver = InstantiateResolver(mergeXml.ResultSet);

            var factory = new MergingFactory();

            IMergingArgs args;
            switch (mergeXml)
            {
                case UnionXml union: args = new UnionArgs(innerResolver, union.ColumnIdentity); break;
                default: args = new CartesianProductArgs(innerResolver); break;
            }

            var merge = factory.Instantiate(args);
            return merge;
        }

        private IAlteration InstantiateTransform(TransformXml transformXml)
        {
            var provider = new TransformationProvider(new ServiceLocator(), new Context(Variables));
            provider.Add(transformXml.Identifier, transformXml);
            return provider;
        }

        private IAlteration InstantiateSummarize(SummarizeXml summarizeXml)
        {
            var scalarHelper = new ScalarHelper(ServiceLocator, null);

            var factory = new SummarizationFactory();
            var aggregations = new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(
                            (summarizeXml.Aggregation as ColumnAggregationXml)?.Identifier,
                            summarizeXml.Aggregation.Function,
                            summarizeXml.Aggregation.ColumnType,
                            summarizeXml.Aggregation.Parameters.Select(x => scalarHelper.InstantiateResolver(summarizeXml.Aggregation.ColumnType, x)).ToList()
                        )
                    };
            var groupBys = summarizeXml.GroupBy?.Columns?.Cast<IColumnDefinitionLight>() ?? new List<IColumnDefinitionLight>();

            var summarization = factory.Instantiate(new SummarizeArgs(aggregations, groupBys));
            return summarization;
        }

        private IAlteration InstantiateExtend(ExtendXml extendXml)
        {
            var factory = new ExtensionFactory(ServiceLocator, new Context(Variables));
            var extension = factory.Instantiate(new ExtendArgs
                (
                    extendXml.Identifier
                    , extendXml.Script?.Code ?? throw new ArgumentException("Script cannot be empty or null")
                    , extendXml.Script.Language
                ));
            return extension;
        }

        private IAlteration InstantiateUnstack(UnstackXml unstackXml)
        {
            var factory = new ReshapingFactory();
            var header = unstackXml.Header.Column.Identifier;
            var groupBys = unstackXml.GroupBy?.Columns?.Cast<IColumnDefinitionLight>() ?? new List<IColumnDefinitionLight>();
            var values = unstackXml.Header.EnforcedValues.Select(x => new ColumnNameIdentifier(x));

            var reshaping = factory.Instantiate(new UnstackArgs(header, groupBys, values));
            return reshaping;
        }

        private IAlteration InstantiateProject(ProjectXml projectXml)
        {
            var factory = new ProjectionFactory();
            var projection = factory.Instantiate(new ProjectArgs(projectXml.Columns.Select(x => x.Identifier)));
            return projection;
        }

        private IAlteration InstantiateProjectAway(ProjectAwayXml projectXml)
        {
            var factory = new ProjectionFactory();
            var projection = factory.Instantiate(new ProjectAwayArgs(projectXml.Columns.Select(x => x.Identifier)));
            return projection;
        }

        private IAlteration InstantiateDuplicate(DuplicateXml duplicateXml)
        {
            var context = new Context(Variables);

            //Predication
            var predicationFactory = new PredicationFactory();
            var predication = predicationFactory.True;
            if (duplicateXml.Predication != null)
            {
                var helper = new PredicateArgsBuilder(ServiceLocator, context);
                var predicateArgs = helper.Execute(duplicateXml.Predication.ColumnType, duplicateXml.Predication.Predicate);
                var predicateFactory = new PredicateFactory();
                var predicate = predicateFactory.Instantiate(predicateArgs);

                predication = predicationFactory.Instantiate(predicate, duplicateXml.Predication.Operand);
            }

            //Times
            var times = new ScalarHelper(ServiceLocator, context).InstantiateResolver<int>(duplicateXml.Times);

            //Outputs
            var outputs = new List<OutputArgs>();
            foreach (var outputXml in duplicateXml.Outputs)
                if (outputXml.Class == OutputClass.Script)
                    outputs.Add(new OutputScriptArgs(ServiceLocator, context, outputXml.Identifier, outputXml.Script.Language, outputXml.Script.Code));
                else if (outputXml.Class == OutputClass.Static)
                    outputs.Add(new OutputValueArgs(outputXml.Identifier, outputXml.Value));
                else
                    outputs.Add(new OutputArgs(outputXml.Identifier, outputXml.Class));

            //Duplicate
            var args = new DuplicateArgs(predication, times, outputs);
            var factory = new DuplicationFactory(ServiceLocator, context);
            var duplication = factory.Instantiate(args);
            return duplication;
        }

        private IAlteration InstantiateLookupReplace(LookupReplaceXml lookupReplaceXml, SettingsXml settingsXml)
        {
            lookupReplaceXml.ResultSet.Settings = settingsXml;
            var innerResolver = InstantiateResolver(lookupReplaceXml.ResultSet);

            var factory = new LookupFactory();

            IMissingStrategy strategy = new FailureMissingStrategy();
            switch (lookupReplaceXml.Missing.Behavior)
            {
                case Behavior.OriginalValue:
                    strategy = new OriginalValueMissingStrategy();
                    break;
                case Behavior.DefaultValue:
                    strategy = new DefaultValueMissingStrategy(lookupReplaceXml.Missing.DefaultValue);
                    break;
                case Behavior.DiscardRow:
                    strategy = new DiscardRowMissingStrategy();
                    break;
                default:
                    strategy = new FailureMissingStrategy();
                    break;
            }

            var lookup = factory.Instantiate(
                    new LookupReplaceArgs(
                        innerResolver,
                        BuildMappings(lookupReplaceXml.Join).ElementAt(0),
                        lookupReplaceXml.Replacement.Identifier,
                        strategy
                ));

            return lookup;
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