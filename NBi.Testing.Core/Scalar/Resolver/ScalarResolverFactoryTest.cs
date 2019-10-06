using Moq;
using NBi.Core.Injection;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Scalar.Resolver
{
    public class ScalarResolverFactoryTest
    {
        [Test]
        public void Instantiate_LiteralArgs_LiteralResolver()
        {
            var args = new LiteralScalarResolverArgs("myValue");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<LiteralScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_GlobalVariableArgs_GlobalVariableResolver()
        {
            var args = new GlobalVariableScalarResolverArgs("myVar", new Dictionary<string, ITestVariable>() { { "myVar", null } });

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_ContextArgs_ContextResolver()
        {
            using (var dt = new DataTable())
            {
                var context = new Context();
                context.Switch(dt.NewRow());
                var args = new ContextScalarResolverArgs(context, new ColumnOrdinalIdentifier(0));

                var factory = new ScalarResolverFactory(null);
                var resolver = factory.Instantiate(args);

                Assert.That(resolver, Is.TypeOf<ContextScalarResolver<object>>());
            }
        }

        [Test]
        public void Instantiate_QueryArgs_QueryResolver()
        {
            var args = new QueryScalarResolverArgs(new EmbeddedQueryResolverArgs("select * from table;", "connStr", null, null, new TimeSpan()));

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<QueryScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_ProjectionResultSetArgs_ProjectionResultSetResolver()
        {
            var args = new RowCountResultSetScalarResolverArgs(new ResultSetResolverArgs());
            var stub = new Mock<ServiceLocator>();
            stub.Setup(x => x.GetResultSetResolverFactory()).Returns(new ResultSetResolverFactory(stub.Object));

            var factory = new ScalarResolverFactory(stub.Object);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<ProjectionResultSetScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_CSharpArgs_CSharpResolver()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<CSharpScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_EnvironmentArgs_EnvironmentResolver()
        {
            var args = new EnvironmentScalarResolverArgs("myVar");

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<EnvironmentScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Dictionary<string, ITestVariable>());

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate<string>(args);

            Assert.That(resolver, Is.TypeOf<FormatScalarResolver>());
        }

        [Test]
        public void InstantiateNeutral_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Dictionary<string, ITestVariable>());

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<FormatScalarResolver>());
        }

        [Test]
        public void InstantiateNotString_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Dictionary<string, ITestVariable>());

            var factory = new ScalarResolverFactory(null);
            var ex = Assert.Throws<ArgumentException>(() => factory.Instantiate<object>(args));
        }

        [Test]
        public void Instantiate_FunctionArgs_FunctionResolver()
        {
            var args = new FunctionScalarResolverArgs(new LiteralScalarResolver<string>("myVar"), new INativeTransformation[] { });

            var factory = new ScalarResolverFactory(null);
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<FunctionScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_NCalcArgs_NcalcResolver()
        {
            using (var dt = new DataTable())
            {
                var row = dt.NewRow();
                var args = new NCalcScalarResolverArgs("a * b - 2", row);

                var factory = new ScalarResolverFactory(null);
                var resolver = factory.Instantiate(args);

                Assert.That(resolver, Is.TypeOf<NCalcScalarResolver<object>>());
            }
        }
    }
}
