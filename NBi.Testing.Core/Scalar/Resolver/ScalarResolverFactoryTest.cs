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

namespace NBi.Core.Testing.Scalar.Resolver
{
    public class ScalarResolverFactoryTest
    {
        [Test]
        public void Instantiate_LiteralArgs_LiteralResolver()
        {
            var args = new LiteralScalarResolverArgs("myValue");

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<LiteralScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_GlobalVariableArgs_GlobalVariableResolver()
        {
            var args = new GlobalVariableScalarResolverArgs("myVar", new Context(new Dictionary<string, IVariable>() { { "myVar", new GlobalVariable(new LiteralScalarResolver<int>(0)) } }));

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_ContextArgs_ContextResolver()
        {
            using var dt = new DataTableResultSet();
            var context = Context.None;
            context.Switch(dt.NewRow());
            var args = new ContextScalarResolverArgs(context, new ColumnOrdinalIdentifier(0));

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<ContextScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_QueryArgs_QueryResolver()
        {
            var args = new QueryScalarResolverArgs(new EmbeddedQueryResolverArgs("select * from table;", "connStr", [], [], new TimeSpan()));

            var factory = new ScalarResolverFactory();
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

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<CSharpScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_EnvironmentArgs_EnvironmentResolver()
        {
            var args = new EnvironmentScalarResolverArgs("myVar");

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<EnvironmentScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Context(new Dictionary<string, IVariable>()));

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate<string>(args);

            Assert.That(resolver, Is.TypeOf<FormatScalarResolver>());
        }

        [Test]
        public void InstantiateNeutral_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Context(new Dictionary<string, IVariable>()));

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<FormatScalarResolver>());
        }

        [Test]
        public void InstantiateNotString_FormatArgs_FormatResolver()
        {
            var args = new FormatScalarResolverArgs("myVar", new Context(new Dictionary<string, IVariable>()));

            var factory = new ScalarResolverFactory();
            var ex = Assert.Throws<ArgumentException>(() => factory.Instantiate<object>(args));
        }

        [Test]
        public void Instantiate_FunctionArgs_FunctionResolver()
        {
            var args = new FunctionScalarResolverArgs(new LiteralScalarResolver<string>("myVar"), Array.Empty<INativeTransformation>());

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<FunctionScalarResolver<object>>());
        }

        [Test]
        public void Instantiate_NCalcArgs_NcalcResolver()
        {
            var context = Context.None;
            var args = new NCalcScalarResolverArgs("a * b - 2", context);

            var factory = new ScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<NCalcScalarResolver<object>>());
        }
    }
}
