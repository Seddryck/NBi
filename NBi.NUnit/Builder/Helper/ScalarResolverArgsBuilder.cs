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
using NBi.Xml.Variables.Custom;
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
        private IScalarResolverArgs args = null;

        private ServiceLocator ServiceLocator { get; }
        private Context Context { get; }

        public ScalarResolverArgsBuilder(ServiceLocator serviceLocator, Context context) 
            => (ServiceLocator, Context) = (serviceLocator, context);

        public void Setup(object obj)
            => Setup(obj, null, SettingsXml.DefaultScope.Everywhere);

        public void Setup(object obj, SettingsXml settings, SettingsXml.DefaultScope scope)
        {
            this.obj = obj;
            this.settings = settings ?? SettingsXml.Empty;
            this.scope = scope;
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
                    var queryBuilder = new QueryResolverArgsBuilder(ServiceLocator);
                    queryBuilder.Setup(obj, settings, scope, Context.Variables);
                    queryBuilder.Build();
                    args = new QueryScalarResolverArgs(queryBuilder.GetArgs());
                    break;
                case ProjectionOldXml obj:
                    var resultSetBuilder = new ResultSetResolverArgsBuilder(ServiceLocator);
                    resultSetBuilder.Setup(obj.ResultSet, settings, scope, Context.Variables);
                    resultSetBuilder.Build();
                    args = new RowCountResultSetScalarResolverArgs(resultSetBuilder.GetArgs());
                    break;
                case EnvironmentXml obj:
                    args = new EnvironmentScalarResolverArgs(obj.Name);
                    break;
                case CustomXml obj:
                    var helper = new ScalarHelper(ServiceLocator, Context);
                    args = new CustomScalarResolverArgs(
                            helper.InstantiateResolver<string>(obj.AssemblyPath),
                            helper.InstantiateResolver<string>(obj.TypeName),
                            obj.Parameters.Select(x => new { x.Name, ScalarResolver = (IScalarResolver)helper.InstantiateResolver<string>(x.StringValue)})
                            .ToDictionary(x => x.Name, y => y.ScalarResolver)
                        );
                    break;
                default:
                    var factory = new ScalarResolverArgsFactory(ServiceLocator, Context);
                    args = factory.Instantiate(obj as string);
                    break;
            }
        }

        public IScalarResolverArgs GetArgs() => args;
    }
}
