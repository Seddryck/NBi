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
        private SettingsXml.DefaultScope scope = SettingsXml.DefaultScope.Everywhere;
        private IDictionary<string, ITestVariable> variables = new Dictionary<string, ITestVariable>();
        private IScalarResolverArgs args = null;

        private readonly ServiceLocator serviceLocator;

        public ScalarResolverArgsBuilder(ServiceLocator serviceLocator) 
            => this.serviceLocator = serviceLocator;

        public void Setup(object obj, IDictionary<string, ITestVariable> variables)
            => Setup(obj, null, SettingsXml.DefaultScope.Everywhere, variables);

        public void Setup(object obj, SettingsXml settings, SettingsXml.DefaultScope scope,IDictionary<string, ITestVariable> variables)
        {
            this.obj = obj;
            this.settings = settings ?? SettingsXml.Empty;
            this.scope = scope;
            this.variables = variables ?? new Dictionary<string, ITestVariable>();
            isSetup = true;
        }

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
                    queryBuilder.Setup(obj, settings, scope, variables);
                    queryBuilder.Build();
                    args = new QueryScalarResolverArgs(queryBuilder.GetArgs());
                    break;
                case ProjectionXml obj:
                    var resultSetBuilder = new ResultSetResolverArgsBuilder(serviceLocator);
                    resultSetBuilder.Setup(obj.ResultSet, settings, scope, variables);
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
