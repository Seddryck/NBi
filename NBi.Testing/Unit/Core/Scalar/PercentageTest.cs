using NBi.Core.Scalar;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar
{
    public class PercentageTest
    {
        [Test]
        public void Convert_FromStringWithPercentage_Correct()
        {
            string value = "50%";
            var converter = TypeDescriptor.GetConverter(typeof(Percentage));
            Assert.That(converter.CanConvertFrom(value.GetType()));
            var pc = converter.ConvertFrom(value);
            Assert.That(pc, Is.TypeOf<Percentage>());
            Assert.That(((Percentage)pc).Value, Is.EqualTo(0.5));
        }

        [Test]
        [TestCase("0.4", 0.4)]
        [TestCase("40", 40)]
        public void Convert_FromStringWithoutPercentage_CorrectValue(string value, double result)
        {
            var converter = TypeDescriptor.GetConverter(typeof(Percentage));
            Assert.That(converter.CanConvertFrom(value.GetType()));
            var pc = converter.ConvertFrom(value);
            Assert.That(pc, Is.TypeOf<Percentage>());
            Assert.That(((Percentage)pc).Value, Is.EqualTo(result));
        }

        [Test]
        [TestCase("0.4", "40%")]
        [TestCase("40", "4000%")]
        [TestCase("50%", "50%")]
        [TestCase("0.500%", "0.5%")]
        public void Convert_FromStringWithoutPercentage_CorrectString(string value, string result)
        {
            var converter = TypeDescriptor.GetConverter(typeof(Percentage));
            Assert.That(converter.CanConvertFrom(value.GetType()));
            var pc = converter.ConvertFrom(value);
            Assert.That(pc, Is.TypeOf<Percentage>());
            Assert.That(((Percentage)pc).ToString(), Is.EqualTo(result));
        }

    }
}
