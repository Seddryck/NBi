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
        public void Instantiate_CSharp_CSharpTestVariable()
        {
            var factory = new TestVariableFactory();
            var variable = factory.Instantiate(LanguageType.CSharp, "DateTime.Now.Year");

            Assert.That(variable, Is.TypeOf<CSharpTestVariable>());
            Assert.That(((variable as CSharpTestVariable).Code), Is.EqualTo("DateTime.Now.Year"));
        }

        [Test]
        public void Instantiate_CSharp_IsNotEvaluated()
        {
            var factory = new TestVariableFactory();
            var variable = factory.Instantiate(LanguageType.CSharp, "DateTime.Now.Year");

            Assert.That(variable.IsEvaluated, Is.False);
        }
    }
}
