using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
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
        [TestCase("")]
        [TestCase("(null)")]
        [TestCase("\t")]
        [TestCase(" \t")]
        [TestCase(" ")]
        [TestCase("\r\n")]
        public void Execute_BlankToNull_Null(string value)
        {
            var function = new BlankToNull();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase("(null)")]
        [TestCase("alpha")]
        public void Execute_BlankToEmpty_NotEmpty(string value)
        {
            var function = new BlankToNull();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("(empty)"));
        }

        [Test]
        [TestCase("alpha")]
        public void Execute_BlankToNull_NotNull(string value)
        {
            var function = new BlankToNull();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("(null)"));
        }

        [Test]
        [TestCase("")]
        [TestCase("(null)")]
        [TestCase("(empty)")]
        public void Execute_EmptyToNull_Null(string value)
        {
            var function = new EmptyToNull();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase("alpha")]
        [TestCase("\t")]
        [TestCase(" \t")]
        [TestCase(" ")]
        [TestCase("\r\n")]
        public void Execute_EmptyToNull_NotNull(string value)
        {
            var function = new EmptyToNull();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("(null)"));
        }


        [Test]
        [TestCase("")]
        [TestCase("(any)")]
        [TestCase("(empty)")]
        [TestCase("(null)")]
        [TestCase(150)]
        public void Execute_AnyToAny_Any(object value)
        {
            var function = new AnyToAny();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("(any)"));
        }


        [Test]
        [TestCase("")]
        [TestCase("(any)")]
        [TestCase("(empty)")]
        [TestCase(150)]
        public void Execute_ValueToValue_Value(object value)
        {
            var function = new ValueToValue();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(null)")]
        public void Execute_ValueToValue_NotValue(object value)
        {
            var function = new ValueToValue();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(null)")]
        public void Execute_NullToValue_Value(object value)
        {
            var function = new NullToValue();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(empty)")]
        [TestCase("123")]
        public void Execute_NullToValue_NotValue(object value)
        {
            var function = new NullToValue();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("(value)"));
        }

        [Test]
        [TestCase("ABC")]
        [TestCase(" ABC ")]
        public void Execute_Trim_ABC(object value)
        {
            var function = new TextToTrim();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("ABC"));
        }

        [Test]
        [TestCase("(null)")]
        [TestCase(" XYZ ")]
        public void Execute_Trim_NotABC(object value)
        {
            var function = new TextToTrim();
            var result = function.Evaluate(value);
            Assert.That(result, Is.Not.EqualTo("ABC"));
        }


        [Test]
        [TestCase("abC")]
        public void Execute_UpperCase_ABC(object value)
        {
            var function = new TextToUpper();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("ABC"));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_LowerCase_abc(object value)
        {
            var function = new TextToLower();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(" abc "));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_Length_5(object value)
        {
            var function = new TextToLength();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        [TestCase("Cédric")]
        public void Execute_TextToHtml_Valid(object value)
        {
            var function = new TextToHtml();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("C&#233;dric"));
        }

        [Test]
        [TestCase("C&#233;dric")]
        [TestCase("C&eacute;dric")]
        public void Execute_HtmlToText_Valid(object value)
        {
            var function = new HtmlToText();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("Cédric"));
        }

        [Test]
        [TestCase("Cédric")]
        public void Execute_Diacritics_Valid(object value)
        {
            var function = new TextToWithoutDiacritics();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo("Cedric"));
        }

        [Test]
        [TestCase("My taylor is rich", "Mytaylorisrich")]
        [TestCase(" My Lord ! ", "MyLord!")]
        [TestCase("My Lord !\r\nMy taylor is \t rich", "MyLord!Mytaylorisrich")]
        [TestCase("(null)", "(null)")]
        [TestCase(null, "(null)")]
        [TestCase("(empty)", "(empty)")]
        [TestCase("(blank)", "(empty)")]
        public void Execute_Whitespace_Valid(object value, string expected)
        {
            var function = new TextToWithoutWhitespaces();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("My taylor is rich", 4)]
        [TestCase(" My Lord ! ", 2)]
        [TestCase("  My     Lord    !   ", 2)]
        [TestCase("  My     Lord    !   C-L.", 3)]
        [TestCase("(null)", 0)]
        [TestCase(null, 0)]
        [TestCase("(empty)", 0)]
        [TestCase("(blank)", 0)]
        public void Execute_TokenCount_Valid(object value, int expected)
        {
            var function = new TextToTokenCount();
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", "abc", "abc123456789")]
        [TestCase("(null)", "abc", "(null)")]
        [TestCase("(empty)", "abc", "abc")]
        [TestCase("(blank)", "abc", "abc")]
        public void Execute_TextToPrefix_Valid(string value, string prefix, string expected)
        {
            var function = new TextToPrefix(new LiteralScalarResolver<string>(prefix));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", "abc", "123456789abc")]
        [TestCase("(null)", "abc", "(null)")]
        [TestCase("(empty)", "abc", "abc")]
        [TestCase("(blank)", "abc", "abc")]
        public void Execute_TextToSuffix_Valid(string value, string suffix, string expected)
        {
            var function = new TextToSuffix(new LiteralScalarResolver<string>(suffix));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", 9, "123456789")]
        [TestCase("123456789", 10, "123456789")]
        [TestCase("123456789", 8, "12345678")]
        [TestCase("123456789", 0, "")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToFirstChars_Valid(string value, int length, string expected)
        {
            var function = new TextToFirstChars(new LiteralScalarResolver<int>(length));
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
            var function = new TextToLastChars(new LiteralScalarResolver<int>(length));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", 9, "(empty)")]
        [TestCase("123456789", 10, "(empty)")]
        [TestCase("123456789", 8, "9")]
        [TestCase("123456789", 5, "6789")]
        [TestCase("123456789", 0, "123456789")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToSkipFirstChars_Valid(string value, int length, string expected)
        {
            var function = new TextToSkipFirstChars(new LiteralScalarResolver<int>(length));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", 9, "(empty)")]
        [TestCase("123456789", 10, "(empty)")]
        [TestCase("123456789", 8, "1")]
        [TestCase("123456789", 5, "1234")]
        [TestCase("123456789", 0, "123456789")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToSkipLastChars_Valid(string value, int length, string expected)
        {
            var function = new TextToSkipLastChars(new LiteralScalarResolver<int>(length));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Execute_TextToLastCharsWithVariable_Valid()
        {
            var args = new GlobalVariableScalarResolverArgs("length", new Dictionary<string, ITestVariable>() { { "length", new GlobalVariable(new LiteralScalarResolver<int>(6) )} });
            var function = new TextToLastChars(new GlobalVariableScalarResolver<int>(args));
            var result = function.Evaluate("123456789");
            Assert.That(result, Is.EqualTo("456789"));
        }

        [Test]
        [TestCase("1234", 9, '0', "123400000")]
        [TestCase("1234", 9, '*', "1234*****")]
        [TestCase("123456789", 3, '0', "123456789")]
        [TestCase("(null)", 3, '0', "000")]
        [TestCase("(empty)", 3, '0', "000")]
        public void Execute_TextToPadRight_Valid(string value, int length, char character, string expected)
        {
            var function = new TextToPadRight(new LiteralScalarResolver<int>(length), new LiteralScalarResolver<char>(character));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1234", 9, '0', "000001234")]
        [TestCase("1234", 9, '*', "*****1234")]
        [TestCase("123456789", 3, '0', "123456789")]
        [TestCase("(null)", 3, '0', "000")]
        [TestCase("(empty)", 3, '0', "000")]
        public void Execute_TextToPadLeft_Valid(string value, int length, char character, string expected)
        {
            var function = new TextToPadLeft(new LiteralScalarResolver<int>(length), new LiteralScalarResolver<char>(character));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("20190317111223", "yyyyMMddhhmmss", "2019-03-17 11:12:23")]
        [TestCase("2019-03-17 11:12:23", "yyyy-MM-dd hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17-03-2019 11:12:23", "dd-MM-yyyy hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("2019-03-17T11:12:23", "yyyy-MM-ddThh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17/03/2019 11:12:23", "dd/MM/yyyy hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17.03.2019 11.12.23", "dd.MM.yyyy hh.mm.ss", "2019-03-17 11:12:23")]
        [TestCase("Wed, 25.09.19", "ddd, dd.MM.yy", "2019-09-25")]
        [TestCase("Wednesday 25-SEP-19", "dddd dd-MMM-yy", "2019-09-25")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "2019-10-01 19:58:00")]
        public void Execute_TextToDateTime_Valid(string value, string format, DateTime expected)
        {
            var function = new TextToDateTime(new LiteralScalarResolver<string>(format));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
            Assert.That(((DateTime) result).Kind, Is.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        [TestCase("2019-11-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Brussels", "2019-11-01 20:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Brussels", "2019-10-01 21:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Moscow", "2019-10-01 22:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Pacific Standard Time", "2019-10-01 12:58:00")]
        public void Execute_TextToDateTimeAndUtcToLocal_Valid(string value, string format, string timeZone, DateTime expected)
        {
            var textToDateTime = new TextToDateTime(new LiteralScalarResolver<string>(format));
            var utcToLocal = new UtcToLocal(new LiteralScalarResolver<string>(timeZone));
            var result = utcToLocal.Evaluate(textToDateTime.Evaluate(value));
            Assert.That(result, Is.EqualTo(expected));
            Assert.That(((DateTime)result).Kind, Is.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        [TestCase("20190317111223", "yyyyMMddhhmmss", "fr-fr", "2019-03-17 11:12:23")]
        [TestCase("mercredi 25-sept.-19", "dddd dd-MMM-yy", "fr-fr", "2019-09-25")]
        public void Execute_TextToDateTimeWithCulture_Valid(string value, string format, string culture, DateTime expected)
        {
            var function = new TextToDateTime(new LiteralScalarResolver<string>(format), new LiteralScalarResolver<string>(culture));
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
