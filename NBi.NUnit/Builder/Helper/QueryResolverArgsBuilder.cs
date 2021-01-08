using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using System.Text.RegularExpressions;
using System.Diagnostics;
using NBi.Core;

namespace NBi.NUnit.Builder.Helper
{
    class QueryResolverArgsBuilder
    {
        private bool isSetup = false;
        private readonly ServiceLocator serviceLocator;

        private object obj = null;
        protected SettingsXml Settings { get; private set; } = SettingsXml.Empty;
        protected SettingsXml.DefaultScope Scope { get; private set; } = SettingsXml.DefaultScope.Everywhere;
        protected IDictionary<string, IVariable> Variables { get; private set; } = new Dictionary<string, IVariable>();
        private BaseQueryResolverArgs args = null;

        public QueryResolverArgsBuilder(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Setup(QueryXml queryXml, SettingsXml settingsXml, SettingsXml.DefaultScope scope, IDictionary<string, IVariable> variables)
        {
            obj = queryXml;
            Settings = settingsXml ?? SettingsXml.Empty;
            Scope = scope;
            Variables = variables;
            isSetup = true;
        }

        public void Setup(ExecutableXml executableXml, SettingsXml settingsXml, IDictionary<string, IVariable> variables)
        {
            obj = executableXml;
            Settings = settingsXml ?? SettingsXml.Empty;
            Scope = SettingsXml.DefaultScope.SystemUnderTest;
            Variables = variables;
            isSetup = true;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            switch (obj)
            {
                case QueryXml queryXml: Build(queryXml); break;
                case ExecutableXml executableXml: Build(executableXml); break;
            }
        }

        protected void Build(QueryXml queryXml)
        {
            queryXml.Settings = Settings;
            var connectionString = new ConnectionStringHelper().Execute(queryXml, Scope);
            var parameters = BuildParameters(queryXml.GetParameters());
            var templateVariables = queryXml.GetTemplateVariables();
            var timeout = Convert.ToInt32(Math.Ceiling(queryXml.Timeout / 1000m)); //Timeout is expressed in milliseconds

            if (!string.IsNullOrEmpty(queryXml.InlineQuery))
                args = new EmbeddedQueryResolverArgs(queryXml.InlineQuery
                    , connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout));

            else if (!string.IsNullOrEmpty(queryXml.File))
            {
                var file = GetFullPath(Settings?.BasePath, queryXml.File);

                args = new ExternalFileQueryResolverArgs(file
                    , connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout));
            }

            else if (queryXml.Assembly != null)
                args = Build(queryXml.Assembly, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout));


            else if (queryXml.Report != null)
                args = Build(queryXml.Report, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout));


            else if (queryXml.SharedDataset != null)
                args = Build(queryXml.SharedDataset, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout));

            if (args == null)
                throw new ArgumentException();
        }

        private BaseQueryResolverArgs Build(AssemblyXml xml, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> templateVariables, TimeSpan timeout)
        {
            var file = GetFullPath(xml?.Settings?.BasePath, xml.Path);

            return new AssemblyQueryResolverArgs(
                file, xml.Klass, xml.Method,
                xml.Static, xml.GetMethodParameters()
                , connectionString, parameters, templateVariables, timeout);
        }

        private BaseQueryResolverArgs Build(ReportXml xml, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> templateVariables, TimeSpan timeout)
        {
            var path = string.IsNullOrEmpty(xml.Source) ? Settings.BasePath + xml.Path : xml.Path;

            return new ReportDataSetQueryResolverArgs(
                xml.Source, path, xml.Name, xml.Dataset
                , connectionString, parameters, templateVariables, timeout);
        }

        private BaseQueryResolverArgs Build(SharedDatasetXml xml, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> templateVariables, TimeSpan timeout)
        {
            var path = string.IsNullOrEmpty(xml.Source) ? Settings.BasePath + xml.Path : xml.Path;

            return new SharedDataSetQueryResolverArgs(
                xml.Source, path, xml.Name
                , connectionString, parameters, templateVariables, timeout);
        }

        protected void Build(ExecutableXml executableXml)
        {
            if (executableXml is QueryXml)
                Build(executableXml as QueryXml);
            else
            {
                var connectionString = new ConnectionStringHelper().Execute(executableXml, Scope);

                var queryableXml = executableXml as QueryableXml;
                var parameters = BuildParameters(queryableXml.GetParameters());
                var templateVariables = queryableXml.GetTemplateVariables();
                var timeout = queryableXml.Timeout;

                switch (executableXml)
                {
                    case AssemblyXml xml: args = Build(xml, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout)); break;
                    case ReportXml xml: args = Build(xml, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout)); break;
                    case SharedDatasetXml xml: args = Build(xml, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout)); break;
                }
            }
        }

        private string GetFullPath(string basePath, string path)
        {
            if (Path.IsPathRooted(path) || string.IsNullOrEmpty(basePath))
                return path;
            else
                return basePath + path;
        }

        public IEnumerable<IQueryParameter> BuildParameters(IEnumerable<QueryParameterXml> parametersXml)
        {
            foreach (var parameterXml in parametersXml)
            {
                var stringWithoutSpecialChars = parameterXml.StringValue.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

                var builder = new ScalarResolverArgsBuilder(serviceLocator, new Context(Variables));
                builder.Setup(stringWithoutSpecialChars);
                builder.Build();
                var args = builder.GetArgs();

                var factory = serviceLocator.GetScalarResolverFactory();
                var resolver = factory.Instantiate(args);
                yield return new QueryParameter(parameterXml.Name, parameterXml.SqlType, resolver);
            }
        }

        public BaseQueryResolverArgs GetArgs() => args;
    }
}
