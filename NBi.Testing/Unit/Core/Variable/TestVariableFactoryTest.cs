using Moq;
using NBi.Core.Injection;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Variable
{
    public class TestVariableFactoryTest
    {
        [Test]
        public void Instantiate_CSharp_TestVariable()
        {
            var factory = new TestVariableFactory();
            var resolver = new CSharpScalarResolver<object>(new CSharpScalarResolverArgs("DateTime.Now.Year"));
            var variable = factory.Instantiate(resolver);

            Assert.That(variable, Is.AssignableTo<ITestVariable>());
            Assert.That(variable, Is.TypeOf<TestVariable>());
        }

        [Test]
        public void Instantiate_QueryScalar_TestVariable()
        {
            var factory = new TestVariableFactory();
            var queryResolverArgsMock = new Mock<BaseQueryResolverArgs>(null, null, null, null);
            var resolver = new QueryScalarResolver<object>(new QueryScalarResolverArgs(queryResolverArgsMock.Object), new ServiceLocator());
            var variable = factory.Instantiate(resolver);

            Assert.That(variable, Is.AssignableTo<ITestVariable>());
            Assert.That(variable, Is.TypeOf<TestVariable>());
        }

        [Test]
        public void Instantiate_CSharp_IsNotEvaluated()
        {
            var factory = new TestVariableFactory();
            var resolver = new CSharpScalarResolver<object>(new CSharpScalarResolverArgs("DateTime.Now.Year"));
            var variable = factory.Instantiate(resolver);

            Assert.That(variable.IsEvaluated, Is.False);
        }
    }
}
