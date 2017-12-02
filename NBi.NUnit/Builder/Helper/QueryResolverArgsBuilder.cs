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

namespace NBi.NUnit.Builder.Helper
{
    class QueryResolverArgsBuilder
    {
        private bool isSetup = false;

        private QueryXml queryXml = null;
        private SettingsXml settingsXml = null;
        private IDictionary<string, ITestVariable> globalVariables = null;
        private BaseQueryResolverArgs args = null;

        public void Setup(QueryXml queryXml)
        {
            this.queryXml = queryXml;
            isSetup = true;
        }

        public void Setup(SettingsXml settingsXml)
        {
            this.settingsXml = settingsXml;
        }

        public void Setup(IDictionary<string, ITestVariable> globalVariables)
        {
            this.globalVariables = globalVariables;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            var connectionString = queryXml.GetConnectionString();
            var parameters = BuildParameters(queryXml.GetParameters());
            var variables = queryXml.GetVariables();
            var timeout = queryXml.Timeout;

            if (!string.IsNullOrEmpty(queryXml.InlineQuery))
                args = new EmbeddedQueryResolverArgs(queryXml.InlineQuery
                    , connectionString, parameters, variables, new TimeSpan(0, 0, timeout));

            else if (!string.IsNullOrEmpty(queryXml.File))
            {
                var file = GetFullPath(settingsXml?.BasePath, queryXml.File);

                args = new ExternalFileQueryResolverArgs(file
                    , connectionString, parameters, variables, new TimeSpan(0, 0, timeout));
            }

            else if (queryXml.Assembly != null)
            {
                var file = GetFullPath(settingsXml?.BasePath, queryXml.Assembly.Path);

                args = new AssemblyQueryResolverArgs(
                    file, queryXml.Assembly.Klass, queryXml.Assembly.Method,
                    queryXml.Assembly.Static, queryXml.Assembly.GetMethodParameters()
                    , connectionString, parameters, variables, new TimeSpan(0, 0, timeout));
            }

            else if (queryXml.Report != null)
            {
                var path = string.IsNullOrEmpty(queryXml.Report.Source) ? settingsXml.BasePath + queryXml.Report.Path : queryXml.Report.Path;

                args = new ReportDataSetQueryResolverArgs(
                    queryXml.Report.Source, path, queryXml.Report.Name, queryXml.Report.Dataset
                    , connectionString, parameters, variables, new TimeSpan(0, 0, timeout));
            }

            else if (queryXml.SharedDataset != null)
            {
                var path = string.IsNullOrEmpty(queryXml.SharedDataset.Source) ? settingsXml.BasePath + queryXml.SharedDataset.Path : queryXml.SharedDataset.Path;

                args = new SharedDataSetQueryResolverArgs(
                    queryXml.SharedDataset.Source, queryXml.SharedDataset.Path, queryXml.SharedDataset.Name
                    , connectionString, parameters, variables, new TimeSpan(0, 0, timeout));
            }

            if (args == null)
                throw new ArgumentException();
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

                var builder = new ScalarResolverArgsBuilder();
                builder.Setup(stringWithoutSpecialChars);
                builder.Setup(globalVariables);
                builder.Build();
                var args = builder.GetArgs();

                var factory = new ScalarResolverFactory();
                var resolver = factory.Instantiate<object>(args);
                yield return new QueryParameter(parameterXml.Name, parameterXml.SqlType, resolver);
            }
        }

        public BaseQueryResolverArgs GetArgs()
        {
            return args;
        }
    }
}
