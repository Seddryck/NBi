using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Xml.Items;
using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Flattening.Xml;
using NBi.Core.DataSerialization.Reader;
using NBi.Core.DataSerialization.Flattening.Json;
using NBi.Xml.Items.Hierarchical.Json;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Combination;
using NBi.Xml.Items.Hierarchical.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NBi.Xml.Variables.Sequence;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Xml.Items.Alteration;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetResolverArgsBuilder
    {
        private bool isSetup = false;

        private object obj = null;
        private SettingsXml settings = null;
        private SettingsXml.DefaultScope scope = SettingsXml.DefaultScope.Everywhere;
        private IDictionary<string, IVariable> Variables { get; set; } = new Dictionary<string, IVariable>();
        private ResultSetResolverArgs args = null;

        private ServiceLocator ServiceLocator { get; }

        public ResultSetResolverArgsBuilder(ServiceLocator serviceLocator) => this.ServiceLocator = serviceLocator;

        public void Setup(object obj, SettingsXml settingsXml, SettingsXml.DefaultScope scope, IDictionary<string, IVariable> variables)
        {
            this.obj = obj;
            this.settings = settingsXml;
            this.scope = scope;
            this.Variables = variables;
            isSetup = true;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            switch (obj)
            {
                case ResultSetSystemXml x: args = BuildResultSetSystemXml(x); break;
                case ResultSetXml x: args = BuildResultSetXml(x); break;
                case IfMissingXml x when !(x?.File?.IsEmpty() ?? false): args = BuildFlatFileResultSetResolverArgs(x.File); break;
                case QueryXml x: args = BuildQueryResolverArgs(x, scope); break;
                case XmlSourceXml x: args = BuildXPathResolverArgs(x); break;
                case JsonSourceXml x: args = BuildJsonPathResolverArgs(x); break;
                default: throw new ArgumentException();
            }
        }

        private ResultSetResolverArgs BuildResultSetSystemXml(ResultSetSystemXml xml)
        {
            if (xml?.IfUnavailable?.ResultSet != null)
                return BuildIfUnavaibleResultSetResolverArgs(BuildInternalResultSetSystemXml(xml), BuildResultSetSystemXml(xml.IfUnavailable.ResultSet));
            else
                return BuildInternalResultSetSystemXml(xml);
        }

        private ResultSetResolverArgs BuildInternalResultSetSystemXml(ResultSetSystemXml xml)
        {
            //ResultSet (external flat file)
            if (!xml?.File?.IsEmpty() ?? false)
                return BuildFlatFileResultSetResolverArgs(xml.File);
            //Query
            else if (xml.Query != null)
                return BuildQueryResolverArgs(xml.Query, scope);
            else if (xml.SequenceCombination != null)
                return BuildSequenceCombinationResolverArgs(xml.SequenceCombination);
            else if (xml.Sequence != null)
                return BuildSequenceResolverArgs(xml.Sequence);
            else if (xml.XmlSource != null)
                return BuildXPathResolverArgs(xml.XmlSource);
            else if (xml.JsonSource != null)
                return BuildJsonPathResolverArgs(xml.JsonSource);
            else if (xml.Empty != null)
                return BuildEmptyResolverArgs(xml.Empty);
            else if (xml.Iteration != null)
                return BuildIterativeResultSetResolverArgs(xml.Iteration, xml.NestedResultSet);
            else if (xml.Rows != null)
                return BuildEmbeddedResolverArgs(xml.Content);

            throw new ArgumentException();
        }

        private ResultSetResolverArgs BuildResultSetXml(ResultSetXml xml)
        {
            //ResultSet (external flat file)
            if (!string.IsNullOrEmpty(xml.File))
            {
                ParseFileInfo(xml.File, out var filename, out var parserName);
                if (string.IsNullOrEmpty(parserName))
                    return BuildFlatFileResultSetResolverArgs(new FileXml() { Path = filename });
                else
                    return BuildFlatFileResultSetResolverArgs(new FileXml() { Path = filename, Parser = new ParserXml() { Name = parserName } });
            }
            //ResultSet (embedded)
            else if (xml.Rows != null)
                return BuildEmbeddedResolverArgs(xml.Content);

            throw new ArgumentException();
        }


        private ResultSetResolverArgs BuildSequenceCombinationResolverArgs(SequenceCombinationXml sequenceCombinationXml)
        {
            var resolvers = new List<ISequenceResolver>();

            var sequenceFactory = new SequenceResolverFactory(ServiceLocator);
            var builder = new SequenceResolverArgsBuilder(ServiceLocator);
            builder.Setup(settings);
            builder.Setup(Variables);

            foreach (var sequenceXml in sequenceCombinationXml.Sequences)
            {
                builder.Setup(sequenceXml.Type);
                builder.Setup((object)sequenceXml.SentinelLoop ?? sequenceXml.Items);
                builder.Build();
                resolvers.Add(sequenceFactory.Instantiate(sequenceXml.Type, builder.GetArgs()));
            }
            return new SequenceCombinationResultSetResolverArgs(resolvers);
        }

        private ResultSetResolverArgs BuildSequenceResolverArgs(SequenceXml sequenceXml)
        {

            var sequenceFactory = new SequenceResolverFactory(ServiceLocator);
            var builder = new SequenceResolverArgsBuilder(ServiceLocator);
            builder.Setup(settings);
            builder.Setup(Variables);
            builder.Setup(sequenceXml.Type);
            builder.Setup((object)sequenceXml.SentinelLoop ?? sequenceXml.Items);
            builder.Build();
            var resolver = sequenceFactory.Instantiate(sequenceXml.Type, builder.GetArgs());
            return new SequenceResultSetResolverArgs(resolver);
        }

        private void ParseFileInfo(string input, out string filename, out string parserName)
        {
            var split = input.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            filename = split[0];
            parserName = split.Count() == 1 ? string.Empty : split[1];
        }

        private ResultSetResolverArgs BuildEmbeddedResolverArgs(IContent content)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet.");
            return new ContentResultSetResolverArgs(content);
        }

        private ResultSetResolverArgs BuildIterativeResultSetResolverArgs(IterationXml iterationXml, ResultSetSystemXml nestedResultSetXml)
        {
            var sequenceFactory = new SequenceResolverFactory(ServiceLocator);
            var builder = new SequenceResolverArgsBuilder(ServiceLocator);
            builder.Setup(settings);
            builder.Setup(Variables);
            builder.Setup(iterationXml.Sequence.Type);
            builder.Setup((object)iterationXml.Sequence.SentinelLoop ?? iterationXml.Sequence.Items);
            builder.Build();
            var sequenceResolver = sequenceFactory.Instantiate(iterationXml.Sequence.Type, builder.GetArgs());

            var nestedHelper = new ResultSetSystemHelper(ServiceLocator, scope, Variables);
            nestedResultSetXml.Settings = settings;
            var nestedResultSetResolver = nestedHelper.InstantiateResolver(nestedResultSetXml);

            return new IterativeResultSetResolverArgs(sequenceResolver, iterationXml.Sequence.Name, Variables, nestedResultSetResolver);
        }

        private ResultSetResolverArgs BuildQueryResolverArgs(QueryXml queryXml, SettingsXml.DefaultScope scope)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined through a query.");
            var argsBuilder = new QueryResolverArgsBuilder(ServiceLocator);
            argsBuilder.Setup(queryXml, settings, scope, Variables);
            argsBuilder.Build();
            var argsQuery = argsBuilder.GetArgs();

            return new QueryResultSetResolverArgs(argsQuery);
        }

        private ResultSetResolverArgs BuildFlatFileResultSetResolverArgs(FileXml fileMetadata)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"ResultSet defined in an external flat file to be read with {(string.IsNullOrEmpty(fileMetadata.Parser?.Name) ? "the default CSV parser" : fileMetadata.Parser?.Name)}.");

            var helper = new ScalarHelper(ServiceLocator, settings, scope, new Context(Variables));
            var resolverPath = helper.InstantiateResolver<string>(fileMetadata.Path);
            if (fileMetadata.IfMissing == null)
                return new FlatFileResultSetResolverArgs(resolverPath, settings?.BasePath, fileMetadata.Parser?.Name, settings?.CsvProfile);

            var builder = new ResultSetResolverArgsBuilder(ServiceLocator);
            builder.Setup(fileMetadata.IfMissing, settings, scope, Variables);
            builder.Build();
            var redirection = builder.GetArgs();
            var factory = new ResultSetResolverFactory(ServiceLocator);
            var resolver = factory.Instantiate(redirection);

            return new FlatFileResultSetResolverArgs(resolverPath, settings?.BasePath, fileMetadata.Parser?.Name, resolver, settings?.CsvProfile);
        }

        private ResultSetResolverArgs BuildXPathResolverArgs(XmlSourceXml xmlSource)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined through an xml-source.");
            var helper = new ScalarHelper(ServiceLocator, settings, scope, new Context(Variables));

            IReaderArgs reader = null;
            if (xmlSource.File != null)
            {
                var resolverPath = helper.InstantiateResolver<string>(xmlSource.File.Path);
                reader = new FileReaderArgs(settings?.BasePath, resolverPath);
            }
            else if (xmlSource.Url != null)
            {
                var resolverUrl = helper.InstantiateResolver<string>(xmlSource.Url.Value);
                reader = new UrlReaderArgs(resolverUrl);
            }
            else if (xmlSource.Rest != null)
            {
                var restHelper = new RestHelper(ServiceLocator, settings, scope, Variables);
                reader = new RestReaderArgs(restHelper.Execute(xmlSource.Rest));
            }

            var selects = new List<IPathSelect>();
            var selectFactory = new PathFlattenizerFactory();
            foreach (var select in xmlSource.XPath.Selects)
                selects.Add(selectFactory.Instantiate(helper.InstantiateResolver<string>(select.Value), select.Attribute, select.Evaluate));
            var flattenizer = new XPathArgs
            {
                From = helper.InstantiateResolver<string>(xmlSource.XPath.From.Value),
                Selects = selects,
                DefaultNamespacePrefix = xmlSource.XPath?.DefaultNamespacePrefix,
                IsIgnoreNamespace = xmlSource.IgnoreNamespace
            };

            return new DataSerializationResultSetResolverArgs(reader, flattenizer);
        }

        private ResultSetResolverArgs BuildJsonPathResolverArgs(JsonSourceXml jsonSource)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined through an json-source.");
            var context = new Context(Variables);
            var helper = new ScalarHelper(ServiceLocator, settings, scope, context);

            IReaderArgs reader = null;
            if (jsonSource.File != null)
            {
                var resolverPath = helper.InstantiateResolver<string>(jsonSource.File.Path);
                reader = new FileReaderArgs(settings?.BasePath, resolverPath);
            }
            else if (jsonSource.Url != null)
            {
                var resolverUrl = helper.InstantiateResolver<string>(jsonSource.Url.Value);
                reader = new UrlReaderArgs(resolverUrl);
            }
            else if (jsonSource.Rest != null)
            {
                var restHelper = new RestHelper(ServiceLocator, settings, scope, Variables);
                reader = new RestReaderArgs(restHelper.Execute(jsonSource.Rest));
            }
            else if (jsonSource.QueryScalar != null)
            {
                var builder = new ScalarResolverArgsBuilder(ServiceLocator, context);
                builder.Setup(jsonSource.QueryScalar, settings, scope);
                builder.Build();
                var args = ServiceLocator.GetScalarResolverFactory().Instantiate<string>(builder.GetArgs());
                reader = new ScalarReaderArgs(args);
            }


            var selects = new List<IPathSelect>();
            var selectFactory = new PathFlattenizerFactory();
            foreach (var select in jsonSource.JsonPath.Selects)
                selects.Add(selectFactory.Instantiate(helper.InstantiateResolver<string>(select.Value), string.Empty, false));
            var flattenizer = new JsonPathArgs
            {
                From = helper.InstantiateResolver<string>(jsonSource.JsonPath.From.Value),
                Selects = selects,
            };

            return new DataSerializationResultSetResolverArgs(reader, flattenizer);
        }

        private ResultSetResolverArgs BuildEmptyResolverArgs(EmptyResultSetXml empty)
        {
            var scalarHelper = new ScalarHelper(ServiceLocator, settings, scope, new Context(Variables));

            if (empty.Columns.Count > 0 && !string.IsNullOrEmpty(empty.ColumnCount))
                return new EmptyResultSetResolverArgs(empty.Columns.Select(x => x.Identifier as ColumnNameIdentifier), scalarHelper.InstantiateResolver<int>(empty.ColumnCount));
            else if (empty.Columns.Count > 0)
                return new EmptyResultSetResolverArgs(empty.Columns.Select(x => x.Identifier as ColumnNameIdentifier));
            else if (!string.IsNullOrEmpty(empty.ColumnCount))
                return new EmptyResultSetResolverArgs(scalarHelper.InstantiateResolver<int>(empty.ColumnCount));
            else
                throw new ArgumentNullException();
        }

        private ResultSetResolverArgs BuildIfUnavaibleResultSetResolverArgs(ResultSetResolverArgs primaryArgs, ResultSetResolverArgs secondaryArgs)
        {
            var factory = ServiceLocator.GetResultSetResolverFactory();
            var primary = factory.Instantiate(primaryArgs);
            var secondary = factory.Instantiate(secondaryArgs);
            return new IfUnavailableResultSetResolverArgs(primary, secondary);
        }

        public ResultSetResolverArgs GetArgs() => args;
    }
}
