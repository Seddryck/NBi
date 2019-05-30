using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Core.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Combination;
using NBi.Xml.Items.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class ResultSetResolverArgsBuilder
    {
        private bool isSetup = false;

        private object obj = null;
        private SettingsXml settings = null;
        private IDictionary<string, ITestVariable> globalVariables = new Dictionary<string, ITestVariable>();
        private ResultSetResolverArgs args = null;

        private readonly ServiceLocator serviceLocator;

        public ResultSetResolverArgsBuilder(ServiceLocator serviceLocator) => this.serviceLocator = serviceLocator;

        public void Setup(object obj)
        {
            this.obj = obj;
            isSetup = true;
        }

        public void Setup(SettingsXml settingsXml) => this.settings = settingsXml;

        public void Setup(IDictionary<string, ITestVariable> globalVariables) => this.globalVariables = globalVariables;

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (obj is ResultSetSystemXml)
            {
                //ResultSet (external flat file)
                if (!(obj as ResultSetSystemXml)?.File?.IsEmpty() ?? false)
                    args = BuildFlatFileResultSetResolverArgs((obj as ResultSetSystemXml).File);
                //Query
                else if ((obj as ResultSetSystemXml).Query != null)
                    args = BuildQueryResolverArgs((obj as ResultSetSystemXml).Query);
                //Sequences combination
                else if ((obj as ResultSetSystemXml).SequenceCombination != null)
                    args = BuildSequenceCombinationResolverArgs((obj as ResultSetSystemXml).SequenceCombination);
                //ResultSet (embedded)
                else if ((obj as ResultSetSystemXml).Rows != null)
                    args = BuildEmbeddedResolverArgs((obj as ResultSetSystemXml).Content);
            }

            if (obj is IfMissingXml)
            {
                //ResultSet (external flat file)
                if (!(obj as IfMissingXml)?.File?.IsEmpty() ?? false)
                    args = BuildFlatFileResultSetResolverArgs((obj as IfMissingXml).File);
            }

            if (obj is ResultSetXml)
            {
                //ResultSet (external flat file)
                if (!string.IsNullOrEmpty((obj as ResultSetXml).File))
                {
                    ParseFileInfo((obj as ResultSetXml).File, out var filename, out var parserName);
                    if (string.IsNullOrEmpty(parserName))
                        args = BuildFlatFileResultSetResolverArgs(new FileXml() { Path = filename });
                    else
                        args = BuildFlatFileResultSetResolverArgs(new FileXml() { Path = filename, Parser = new ParserXml() { Name = parserName } });
                }
                //ResultSet (embedded)
                else if ((obj as ResultSetXml).Rows != null)
                    args = BuildEmbeddedResolverArgs((obj as ResultSetXml).Content);
            }

            if (obj is QueryXml)
                args = BuildQueryResolverArgs((obj as QueryXml));

            if (obj is XmlSourceXml)
                args = BuildXPathResolverArgs((obj as XmlSourceXml));

            if (args == null)
                throw new ArgumentException();
        }

        private ResultSetResolverArgs BuildSequenceCombinationResolverArgs(SequenceCombinationXml sequenceCombinationXml)
        {
            var resolvers = new List<ISequenceResolver>();

            var sequenceFactory = new SequenceResolverFactory(serviceLocator);
            var builder = new SequenceResolverArgsBuilder(serviceLocator);
            builder.Setup(settings);
            builder.Setup(globalVariables);

            foreach (var sequenceXml in sequenceCombinationXml.Sequences)
            {
                builder.Setup(sequenceXml.Type);
                builder.Setup((object)sequenceXml.SentinelLoop ?? sequenceXml.Items);
                builder.Build();
                resolvers.Add(sequenceFactory.Instantiate(sequenceXml.Type, builder.GetArgs()));
            }
            return new SequenceCombinationResultSetResolverArgs(resolvers);
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

        private ResultSetResolverArgs BuildQueryResolverArgs(QueryXml queryXml)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined through a query.");
            var argsBuilder = new Helper.QueryResolverArgsBuilder(serviceLocator);
            argsBuilder.Setup(queryXml);
            argsBuilder.Setup(settings);
            argsBuilder.Setup(globalVariables);
            argsBuilder.Build();
            var argsQuery = argsBuilder.GetArgs();

            return new QueryResultSetResolverArgs(argsQuery);
        }

        private ResultSetResolverArgs BuildFlatFileResultSetResolverArgs(FileXml fileMetadata)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"ResultSet defined in an external flat file to be read with {(string.IsNullOrEmpty(fileMetadata.Parser?.Name) ? "the default CSV parser" : fileMetadata.Parser?.Name)}.");

            var helper = new ScalarHelper(serviceLocator, globalVariables, settings);
            var resolverPath = helper.InstantiateResolver<string>(fileMetadata.Path);
            if (fileMetadata.IfMissing == null)
                return new FlatFileResultSetResolverArgs(resolverPath, settings?.BasePath, fileMetadata.Parser?.Name, settings?.CsvProfile);

            var builder = new ResultSetResolverArgsBuilder(serviceLocator);
            builder.Setup(fileMetadata.IfMissing);
            builder.Setup(settings);
            builder.Setup(globalVariables);
            builder.Build();
            var redirection = builder.GetArgs();
            var factory = new ResultSetResolverFactory(serviceLocator);
            var resolver = factory.Instantiate(redirection);

            return new FlatFileResultSetResolverArgs(resolverPath, settings?.BasePath, fileMetadata.Parser?.Name, resolver, settings?.CsvProfile);
        }

        private ResultSetResolverArgs BuildXPathResolverArgs(XmlSourceXml xmlSource)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "ResultSet defined through an xml-source.");

            var selects = new List<AbstractSelect>();
            var selectFactory = new SelectFactory();
            foreach (var select in xmlSource.XPath.Selects)
                selects.Add(selectFactory.Instantiate(select.Value, select.Attribute, select.Evaluate));

            XPathEngine engine = null;
            if (xmlSource.File != null)
                engine = new XPathFileEngine(xmlSource.GetFile(), xmlSource.XPath.From.Value, selects);
            else if (xmlSource.Url != null)
                engine = new XPathUrlEngine(xmlSource.Url.Value, xmlSource.XPath.From.Value, selects);

            return new XPathResultSetResolverArgs(engine);
        }

        public ResultSetResolverArgs GetArgs() => args;
    }
}
