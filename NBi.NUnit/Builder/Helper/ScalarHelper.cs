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
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class ScalarHelper
    {
        private readonly ServiceLocator serviceLocator;
        private readonly IDictionary<string, ITestVariable> variables;

        public ScalarHelper(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables)
        {
            this.serviceLocator = serviceLocator;
            this.variables = variables;
        }

        public IScalarResolver<decimal> InstantiateResolver(ScalarXml scalarXml)
        {
            var argsBuilder = new ScalarResolverArgsBuilder(serviceLocator);
            argsBuilder.Setup(scalarXml);
            argsBuilder.Setup(scalarXml.Settings);
            argsBuilder.Setup(variables);
            argsBuilder.Build();

            var factory = serviceLocator.GetScalarResolverFactory();
            var resolver = factory.Instantiate<decimal>(argsBuilder.GetArgs());
            return resolver;
        }
    }
}
