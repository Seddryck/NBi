using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class ScalarResolverArgsBuilder
    {
        private bool isSetup = false;

        private object obj = null;
        private SettingsXml settings = SettingsXml.Empty;
        private IDictionary<string, ITestVariable> variables = new Dictionary<string, ITestVariable>();
        private IScalarResolverArgs args = null;

        private readonly ServiceLocator serviceLocator;

        public ScalarResolverArgsBuilder(ServiceLocator serviceLocator) => this.serviceLocator = serviceLocator;

        public void Setup(object obj)
        {
            this.obj = obj;
            isSetup = true;
        }

        public void Setup(SettingsXml settings) => this.settings = settings;

        public void Setup(IDictionary<string, ITestVariable> variables) => this.variables = variables;

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            switch (obj)
            {
                case ScriptXml obj when obj.Language == LanguageType.CSharp:
                    args = new CSharpScalarResolverArgs(obj.Code);
                    break;
                case QueryXml obj:
                    var queryBuilder = new QueryResolverArgsBuilder(serviceLocator);
                    queryBuilder.Setup(obj);
                    queryBuilder.Setup(settings);
                    queryBuilder.Setup(variables);
                    queryBuilder.Build();
                    args = new QueryScalarResolverArgs(queryBuilder.GetArgs());
                    break;
                case ProjectionXml obj:
                    var resultSetBuilder = new ResultSetResolverArgsBuilder(serviceLocator);
                    resultSetBuilder.Setup(obj.ResultSet);
                    resultSetBuilder.Setup(settings);
                    resultSetBuilder.Setup(variables);
                    resultSetBuilder.Build();
                    args = new RowCountResultSetScalarResolverArgs(resultSetBuilder.GetArgs());
                    break;
                case EnvironmentXml obj:
                    args = new EnvironmentScalarResolverArgs(obj.Name);
                    break;
                default:
                    var factory = new ScalarResolverArgsFactory(serviceLocator, variables, settings.BasePath);
                    args = factory.Instantiate(obj as string);
                    break;
            }
        }

        public IScalarResolverArgs GetArgs() => args;
    }
}
