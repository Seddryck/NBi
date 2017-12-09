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
    public class CSharpScalarResolverTest
    {
        [Test]
        public void Instantiate_GetValueObject_CorrectComputation()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");
            var resolver = new CSharpScalarResolver<object>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(DateTime.Now.Year));
        }

        [Test]
        public void Instantiate_GetValueInt_CorrectComputation()
        {
            var args = new CSharpScalarResolverArgs("DateTime.Now.Year");
            var resolver = new CSharpScalarResolver<int>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo(DateTime.Now.Year));
        }

    }
}
