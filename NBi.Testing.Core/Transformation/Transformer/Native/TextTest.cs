using NBi.Core.Transformation.Transformer.Native;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Transformation.Transformer.Native
{
    [TestFixture]
    public class TextTest
    {
        [Test]
        [TestCase("123456789", 9, "123456789")]
        [TestCase("123456789", 10, "123456789")]
        [TestCase("123456789", 8, "12345678")]
        [TestCase("123456789", 0, "")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToFirstChars_Valid(string value, int length, string expected)
        {
            var function = new TextToFirstChars(length.ToString());
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", 9, "123456789")]
        [TestCase("123456789", 10, "123456789")]
        [TestCase("123456789", 8, "23456789")]
        [TestCase("123456789", 0, "")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToLastChars_Valid(string value, int length, string expected)
        {
            var function = new TextToLastChars(length.ToString());
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1234", 9, "0", "123400000")]
        [TestCase("1234", 9, "*", "1234*****")]
        [TestCase("123456789", 3, "0", "123456789")]
        [TestCase("(null)", 3, "0", "000")]
        [TestCase("(empty)", 3, "0", "000")]
        public void Execute_TextToPadRight_Valid(string value, int length, string character, string expected)
        {
            var function = new TextToPadRight(length.ToString(), character);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1234", 9, "0", "000001234")]
        [TestCase("1234", 9, "*", "*****1234")]
        [TestCase("123456789", 3, "0", "123456789")]
        [TestCase("(null)", 3, "0", "000")]
        [TestCase("(empty)", 3, "0", "000")]
        public void Execute_TextToPadLeft_Valid(string value, int length, string character, string expected)
        {
            var function = new TextToPadLeft(length.ToString(), character);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
