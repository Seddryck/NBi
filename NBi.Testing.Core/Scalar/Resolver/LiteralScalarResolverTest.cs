using NBi.Core.Scalar;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver
{
    public class LiteralScalarResolverTest
    {
        [Test]
        public void Execute_String_Itself()
        {
            var obj = "My name is Cédric";
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<string>(args);
            Assert.That(resolver.Execute(), Is.EqualTo("My name is Cédric"));
        }

        [Test]
        public void Execute_DoubleToDecimal_Itself()
        {
            double obj = 10.0;
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<decimal>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(obj));
        }

        [Test]
        public void Execute_DecimalToInt_Itself()
        {
            decimal obj = new decimal(10);
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(obj));
        }

        [Test]
        [TestCase("en-us")]
        [TestCase("fr-fr")]
        [TestCase("jp-jp")]
        public void Execute_StringToDecimal_Itself(string culture)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            string obj = "10.0";
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<decimal>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(decimal.Parse(obj, NumberFormatInfo.InvariantInfo)));
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }


        [Test]
        public void Execute_StringToInt32_Itself()
        {
            string obj = "10";
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<decimal>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(Int32.Parse(obj)));
        }

        [Test]
        public void Execute_StringToDateTime_Itself()
        {
            var obj = "2017-11-13 07:05:00";
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<DateTime>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(DateTime.Parse(obj)));
        }

        [Test]
        public void Execute_DateTime_Itself()
        {
            var obj = new DateTime(2017, 11, 13, 7, 5, 0);
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<DateTime>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(obj));
        }

        [Test]
        public void Execute_StringToPercentage_Double()
        {
            var obj = "10%";
            var args = new LiteralScalarResolverArgs(obj);
            var resolver = new LiteralScalarResolver<Percentage>(args);
            Assert.That(resolver.Execute()?.Value, Is.EqualTo(0.1));
        }
    }
}
