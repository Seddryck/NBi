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
                case string obj when !string.IsNullOrEmpty(obj):
                    var tokens = obj.Trim().Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                    var variable = tokens.First().Trim();
                    var prefix = tokens.First().Trim().ToCharArray()[0];
                    var functions = tokens.Skip(1);
                    var factory = serviceLocator.GetScalarResolverFactory();
                    IScalarResolver resolver = null;

                    switch (prefix)
                    {
                        case '@':
                            args = new GlobalVariableScalarResolverArgs(variable.Substring(1), variables);
                            resolver = factory.Instantiate<object>(args);
                            break;
                        case '~':
                            args = new FormatScalarResolverArgs(variable.Substring(1), variables);
                            resolver = factory.Instantiate<string>(args);
                            break;
                        default:
                            args = new LiteralScalarResolverArgs(obj);
                            resolver = factory.Instantiate<object>(args);
                            break;
                    }

                    if (functions.Count() > 0)
                    {
                        var transformations = new List<INativeTransformation>();
                        var nativeTransformationFactory = new NativeTransformationFactory();
                        foreach (var function in functions)
                            transformations.Add(nativeTransformationFactory.Instantiate(function));

                        args = new FunctionScalarResolverArgs(resolver, transformations);
                    }
                    break;
                case null:
                    throw new ArgumentException();
                default:
                    args = new LiteralScalarResolverArgs(obj);
                    break;
            }
        }

        public IScalarResolverArgs GetArgs() => args;
    }
}
