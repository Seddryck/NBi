using NBi.Core.Transformation.CSharp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Transformation.CSharp
{
    [TestFixture]
    public class CSharpTransformationProviderTest
    {
        [Test]
        public void Execute_String_Permuted()
        {
            var code = "value.Substring(3) + \".\" + value.Substring(0,2)";
            var provider = new CSharpTransformationProvider<string>(code);

            var result = provider.Execute("05.2016");
            Assert.That(result, Is.EqualTo("2016.05"));
        }

        [Test]
        public void Execute_Numeric_Multiplied()
        {
            var code = "value * new Decimal(1.21)";
            var provider = new CSharpTransformationProvider<decimal>(code);

            var result = provider.Execute(new Decimal(100));
            Assert.That(result, Is.EqualTo(121));
        }

        [Test]
        public void Execute_Boolean_Translated()
        {
            var code = "value ? \"Oui\" : \"Non\" ";
            var provider = new CSharpTransformationProvider<bool>(code);

            var result = provider.Execute(true);
            Assert.That(result, Is.EqualTo("Oui"));
        }

        [Test]
        public void Execute_DateTime_Formated()
        {
            var code = "String.Format(\"{0:00}.{1}\", value.Month, value.Year)";
            var provider = new CSharpTransformationProvider<DateTime>(code);

            var result = provider.Execute(new DateTime(2016, 5, 17));
            Assert.That(result, Is.EqualTo("05.2016"));
        }

        [Test]
        public void Execute_DateTime_Calculated()
        {
            var code = "(new DateTime(1970,1,1)).AddDays(Convert.ToDouble(value))";
            var provider = new CSharpTransformationProvider<decimal>(code);

            var result = provider.Execute(new decimal(3283));
            Assert.That(result, Is.EqualTo(new DateTime(1978,12,28)));
        }
    }
}
