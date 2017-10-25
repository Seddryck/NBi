using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Variable
{
    public class CSharpTestVariableTest
    {
        [Test]
        public void Instantiate_GetValue_CorrectComputation()
        {
            var variable = new CSharpTestVariable("DateTime.Now.Year");

            var output = variable.GetValue();

            Assert.That(output, Is.EqualTo(DateTime.Now.Year));
        }

        [Test]
        public void Instantiate_GetValue_SetEvaluated()
        {
            var variable = new CSharpTestVariable("DateTime.Now.Year");

            var output = variable.GetValue();

            Assert.That(variable.IsEvaluated, Is.True);
        }
    }
}
