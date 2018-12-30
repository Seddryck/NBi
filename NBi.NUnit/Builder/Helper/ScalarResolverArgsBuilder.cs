using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
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
        private IDictionary<string, ITestVariable> globalVariables = new Dictionary<string, ITestVariable>();
        private IScalarResolverArgs args = null;

        private readonly ServiceLocator serviceLocator;

        public ScalarResolverArgsBuilder(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Setup(object obj)
        {
            this.obj = obj;
            isSetup = true;
        }

        public void Setup(SettingsXml settings)
        {
            this.settings = settings;
        }

        public void Setup(IDictionary<string, ITestVariable> globalVariables)
        {
            this.globalVariables = globalVariables;
        }

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();

            if (obj is ScriptXml && (obj as ScriptXml).Language==LanguageType.CSharp)
            {
                args = new CSharpScalarResolverArgs((obj as ScriptXml).Code);
            }

            else if (obj is QueryXml)
            {
                var builder = new QueryResolverArgsBuilder(serviceLocator);
                builder.Setup((QueryXml)obj);
                builder.Setup(settings);
                builder.Setup(globalVariables);
                builder.Build();
                args = new QueryScalarResolverArgs(builder.GetArgs());
            }

            else if (obj is ProjectionXml)
            {
                var builder = new ResultSetResolverArgsBuilder(serviceLocator);
                builder.Setup(((ProjectionXml)obj).ResultSet);
                builder.Setup(settings);
                builder.Setup(globalVariables);
                builder.Build();
                args = new RowCountResultSetScalarResolverArgs(builder.GetArgs());
            }

            else if (obj is EnvironmentXml)
            {
                args = new EnvironmentScalarResolverArgs((obj as EnvironmentXml).Name);
            }

            else if (obj is string && !string.IsNullOrEmpty((string)obj) && ((string)obj).Trim().StartsWith("@"))
            {
                var variableName = ((string)obj).Trim().Substring(1);
                args = new GlobalVariableScalarResolverArgs(variableName, globalVariables);
            }

            else if (obj is string && !string.IsNullOrEmpty((string)obj) && ((string)obj).Trim().StartsWith("~"))
            {
                var formatText = ((string)obj).Trim().Substring(1);
                args = new FormatScalarResolverArgs(formatText, globalVariables);
            }

            else if (obj is object && obj != null)
            {
                args = new LiteralScalarResolverArgs(obj);
            }

            if (args == null)
                throw new ArgumentException();
        }

        public IScalarResolverArgs GetArgs()
        {
            return args;
        }
    }
}
