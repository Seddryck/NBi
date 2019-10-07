using NBi.Core.Calculation;
using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Ranking;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Conversion;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Xml.Items.Calculation.Ranking;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    public class ScalarHelper
    {
        private ServiceLocator ServiceLocator { get; }
        private Context Context { get; }
        private SettingsXml.DefaultScope Scope { get; }
        private SettingsXml Settings { get; set; }

        public ScalarHelper(ServiceLocator serviceLocator, Context context)
         : this(serviceLocator, null, SettingsXml.DefaultScope.Everywhere, context) { }

        public ScalarHelper(ServiceLocator serviceLocator, SettingsXml settings, SettingsXml.DefaultScope scope, Context context)
        {
            ServiceLocator = serviceLocator;
            Settings = settings;
            Scope = scope;
            Context = context;
        }

        public IScalarResolver<T> InstantiateResolver<T>(ScalarXml scalarXml)
        {
            var argsBuilder = new ScalarResolverArgsBuilder(ServiceLocator, Context);
            argsBuilder.Setup(scalarXml.BaseItem, scalarXml.Settings, Scope);
            argsBuilder.Build();

            var factory = ServiceLocator.GetScalarResolverFactory();
            var resolver = factory.Instantiate<T>(argsBuilder.GetArgs());
            return resolver;
        }

        public IScalarResolver<T> InstantiateResolver<T>(string value)
        {
            var argsBuilder = new ScalarResolverArgsBuilder(ServiceLocator, Context);

            argsBuilder.Setup(value);
            argsBuilder.Build();

            var factory = ServiceLocator.GetScalarResolverFactory();
            var resolver = factory.Instantiate<T>(argsBuilder.GetArgs());
            return resolver;
        }

        public IScalarResolver InstantiateResolver(ColumnType columnType, string value)
        {
            var argsBuilder = new ScalarResolverArgsBuilder(ServiceLocator, Context);

            argsBuilder.Setup(value);
            argsBuilder.Build();
            var args = argsBuilder.GetArgs();

            var factory = ServiceLocator.GetScalarResolverFactory();
            switch (columnType)
            {
                case ColumnType.Text: return factory.Instantiate<string>(args);
                case ColumnType.Numeric: return factory.Instantiate<decimal>(args);
                case ColumnType.DateTime: return factory.Instantiate<DateTime>(args);
                case ColumnType.Boolean: return factory.Instantiate<bool>(args);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
