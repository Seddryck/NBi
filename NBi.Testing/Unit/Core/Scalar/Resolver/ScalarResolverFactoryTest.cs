using Moq;
using NBi.Core.Injection;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Resolver
{
    public class ScalarResolverFactoryTest
    {
        [Test]
        public void Instantiate_LiteralArgs_LiteralResolver()
        {
            var args = new LiteralScalarResolverArgs("myValue");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<LiteralScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_GlobalVariableArgs_GlobalVariableResolver()
        {
            var args = new GlobalVariableScalarResolverArgs("myVar", new Dictionary<string, ITestVariable>() { { "myVar", null } });

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_QueryArgs_QueryResolver()
        {
            var args = new QueryScalarResolverArgs(new EmbeddedQueryResolverArgs("select * from table;", "connStr", null, null, new TimeSpan()));

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<QueryScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_ProjectionResultSetArgs_ProjectionResultSetResolver()
        {
            var args = new RowCountResultSetScalarResolverArgs(new ResultSetResolverArgs());
            var stub = new Mock<ServiceLocator>();
            stub.Setup(x => x.GetResultSetResolverFactory()).Returns(new ResultSetResolverFactory(stub.Object));

            var factory = new ScalarResolverFactory(stub.Object);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<ProjectionResultSetScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_CSharpArgs_CSharpResolver()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<CSharpScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_EnvironmentArgs_EnvironmentResolver()
        {
            var args = new EnvironmentScalarResolverArgs("myVar");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<object>(args);

            Assert.That(resolver, Is.TypeOf<EnvironmentScalarResolver<object>>());
        }
    }
}
