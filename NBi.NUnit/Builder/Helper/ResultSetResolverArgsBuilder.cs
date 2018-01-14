using NBi.Core;
using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Variable;
using NBi.Core.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
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

        public ResultSetResolverArgsBuilder(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Setup(object obj)
        {
            this.obj = obj;
            isSetup = true;
        }

        public void Setup(SettingsXml settingsXml)
        {
            this.settings = settingsXml;
        }

        public void Setup(IDictionary<string, ITestVariable> globalVariables)
        {
            this.globalVariables = globalVariables;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (obj is ResultSetSystemXml)
            {
                //ResultSet (external CSV file)
                if (!string.IsNullOrEmpty((obj as ResultSetSystemXml).File))
                    args = BuildCsvResolverArgs((obj as ResultSetSystemXml).File);
                //Query
                else if ((obj as ResultSetSystemXml).Query != null)
                    args = BuildQueryResolverArgs((obj as ResultSetSystemXml).Query);
                //ResultSet (embedded)
                else if ((obj as ResultSetSystemXml).Rows != null)
                    args = BuildEmbeddedResolverArgs((obj as ResultSetSystemXml).Content);
            }

            if (obj is ResultSetXml)
            {
                //ResultSet (external CSV file)
                if (!string.IsNullOrEmpty((obj as ResultSetXml).File))
                    args = BuildCsvResolverArgs((obj as ResultSetXml).File);
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

        private ResultSetResolverArgs BuildEmbeddedResolverArgs(IContent content)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet.");
            return new ContentResultSetResolverArgs(content);
        }

        private ResultSetResolverArgs BuildQueryResolverArgs(QueryXml queryXml)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined through a query.");
            var argsBuilder = new Helper.QueryResolverArgsBuilder(serviceLocator);
            argsBuilder.Setup(queryXml);
            argsBuilder.Setup(settings);
            argsBuilder.Setup(globalVariables);
            argsBuilder.Build();
            var argsQuery = argsBuilder.GetArgs();

            return new QueryResultSetResolverArgs(argsQuery);
        }

        private ResultSetResolverArgs BuildCsvResolverArgs(string path)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in external CSV file.");
            var file = string.Empty;
            if (Path.IsPathRooted(path))
                file = path;
            else
                file = settings?.BasePath + path;

            return new CsvResultSetResolverArgs(file, settings?.CsvProfile);
        }

        private ResultSetResolverArgs BuildXPathResolverArgs(XmlSourceXml xmlSource)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined through an xml-source.");

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

        public ResultSetResolverArgs GetArgs()
        {
            return args;
        }
    }
}
