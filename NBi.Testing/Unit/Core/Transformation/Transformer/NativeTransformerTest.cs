using NBi.Core.Transformation.Transformer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Transformation.Transformer
{
    [TestFixture]
    public class NativeTransformerTest
    {
        [Test]
        [TestCase("")]
        [TestCase("\t")]
        [TestCase(" \t")]
        [TestCase(" ")]
        [TestCase("\r\n")]
        [TestCase("\r\n \t \r  ")]
        public void Execute_BlankToEmpty_Empty(string value)
        {
            var code = "blank-to-empty";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("(empty)"));
        }

        [Test]
        [TestCase("(null)")]
        [TestCase("alpha")]
        public void Execute_BlankToEmpty_NotEmpty(string value)
        {
            var code = "blank-to-empty";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("(empty)"));
        }

        [Test]
        [TestCase("")]
        [TestCase("(null)")]
        [TestCase("\t")]
        [TestCase(" \t")]
        [TestCase(" ")]
        [TestCase("\r\n")]
        public void Execute_BlankToNull_Null(string value)
        {
            var code = "blank-to-null";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase("alpha")]
        public void Execute_BlankToNull_NotNull(string value)
        {
            var code = "blank-to-null";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("(null)"));
        }

        [Test]
        [TestCase("")]
        [TestCase("(null)")]
        [TestCase("(empty)")]
        public void Execute_EmptyToNull_Null(string value)
        {
            var code = "empty-to-null";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
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
            var code = "empty-to-null";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
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
            var code = "any-to-any";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("(any)"));
        }


        [Test]
        [TestCase("")]
        [TestCase("(any)")]
        [TestCase("(empty)")]
        [TestCase(150)]
        public void Execute_ValueToValue_Value(object value)
        {
            var code = "value-to-value";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(null)")]
        public void Execute_ValueToValue_NotValue(object value)
        {
            var code = "value-to-value";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(null)")]
        public void Execute_NullToValue_Value(object value)
        {
            var code = "null-to-value";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("(value)"));
        }

        [Test]
        [TestCase("(empty)")]
        [TestCase("123")]
        public void Execute_NullToValue_NotValue(object value)
        {
            var code = "null-to-value";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("(value)"));
        }

        [Test]
        [TestCase("ABC")]
        [TestCase(" ABC ")]
        public void Execute_Trim_ABC(object value)
        {
            var code = "text-to-trim";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("ABC"));
        }

        [Test]
        [TestCase("(null)")]
        [TestCase(" XYZ ")]
        public void Execute_Trim_NotABC(object value)
        {
            var code = "text-to-trim";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("ABC"));
        }


        [Test]
        [TestCase("abC")]
        public void Execute_UpperCase_ABC(object value)
        {
            var code = "text-to-upper";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("ABC"));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_LowerCase_abc(object value)
        {
            var code = "text-to-lower";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(" abc "));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_Length_5(object value)
        {
            var code = "text-to-length";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        [TestCase("Cédric")]
        public void Execute_TextToHtml_Valid(object value)
        {
            var code = "text-to-html";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("C&#233;dric"));
        }

        [TestCase(9, 8, 39)]
        [TestCase(12, 28, 39)]
        public void Execute_DateToAge_Min38(int month, int day, int age)
        {
            var code = "date-to-age";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(new DateTime(1978, month, day));
            Assert.That(result, Is.AtLeast(age));
        }

        [Test]
        [TestCase("C&#233;dric")]
        [TestCase("C&eacute;dric")]
        public void Execute_HtmlToText_Valid(object value)
        {
            var code = "html-to-text";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("Cédric"));
        }

        [Test]
        [TestCase("Cédric")]
        public void Execute_Diacritics_Valid(object value)
        {
            var code = "text-to-without-diacritics";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("Cedric"));
        }

        [Test]
        [TestCase("My taylor is rich", "Mytaylorisrich")]
        [TestCase(" My Lord ! ", "MyLord!")]
        [TestCase("My Lord !\r\nMy taylor is \t rich", "MyLord!Mytaylorisrich")]
        [TestCase("(null)", null)]
        [TestCase(null, null)]
        [TestCase("(empty)", "(empty)")]
        [TestCase("(blank)", "(empty)")]
        public void Execute_Whitespace_Valid(object value, string expected)
        {
            var code = "text-to-without-whitespaces";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
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
            var code = "text-to-token-count";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-02-01 01:00:00")]
        [TestCase("2018-08-01 00:00:00", "2018-08-01 02:00:00")]
        public void Execute_UtcToLocalWithStandardName_Valid(object value, DateTime expected)
        {
            var code = "utc-to-local(Romance Standard Time)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-02-01 01:00:00")]
        [TestCase("2018-08-01 00:00:00", "2018-08-01 02:00:00")]
        public void Execute_UtcToLocalWithCityName_Valid(object value, DateTime expected)
        {
            var code = "utc-to-local(Brussels)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("2018-02-01 03:00:00", "2018-02-01 02:00:00")]
        [TestCase("2018-08-01 02:00:00", "2018-08-01 00:00:00")]
        public void Execute_LocalToUtcWithStandardName_Valid(object value, DateTime expected)
        {
            var code = "local-to-utc(Romance Standard Time)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("2018-02-01 07:00:00", "2018-02-01 06:00:00")]
        [TestCase("2018-08-01 01:00:00", "2018-07-31 23:00:00")]
        public void Execute_LocalToUtcWithCityName_Valid(object value, DateTime expected)
        {
            var code = "local-to-utc(Brussels)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 07:00:00")]
        public void Execute_DateTimeToDate_Valid(object value)
        {
            var code = "dateTime-to-date";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(new DateTime(2018, 2, 1)));
        }

        [Test]
        [TestCase("2018-02-01 07:00:00", "2018-02-01 07:00:00")]
        [TestCase(null, "2001-01-01")]
        [TestCase("", "2001-01-01")]
        [TestCase("(null)", "2001-01-01")]
        public void Execute_NullToDate_Valid(object value, DateTime expected)
        {
            var code = "null-to-date(2001-01-01)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-02-01")]
        [TestCase("2018-02-01 07:00:00", "2018-02-01")]
        [TestCase("2018-02-12 07:00:00", "2018-02-01")]
        [TestCase(null, null)]
        [TestCase("(null)", null)]
        public void Execute_DateTimeToFirstOfMonth_Valid(object value, DateTime expected)
        {
            var code = "dateTime-to-first-of-month";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            if (expected == new DateTime(1, 1, 1))
                Assert.That(result, Is.Null);
            else
                Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-01-01")]
        [TestCase("2018-02-01 07:00:00", "2018-01-01")]
        [TestCase("2018-02-12 07:00:00", "2018-01-01")]
        [TestCase(null, null)]
        [TestCase("(null)", null)]
        public void Execute_DateTimeToFirstOfYear_Valid(object value, DateTime expected)
        {
            var code = "dateTime-to-first-of-year";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            if (expected == new DateTime(1, 1, 1))
                Assert.That(result, Is.Null);
            else
                Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-02-28")]
        [TestCase("2018-02-01 07:00:00", "2018-02-28")]
        [TestCase("2018-02-12 07:00:00", "2018-02-28")]
        [TestCase("2020-02-12 07:00:00", "2020-02-29")]
        [TestCase(null, null)]
        [TestCase("(null)", null)]
        public void Execute_DateTimeToLastOfMonth_Valid(object value, DateTime expected)
        {
            var code = "dateTime-to-last-of-month";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            if (expected == new DateTime(1, 1, 1))
                Assert.That(result, Is.Null);
            else
                Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2018-02-01 00:00:00", "2018-12-31")]
        [TestCase("2018-02-01 07:00:00", "2018-12-31")]
        [TestCase("2018-02-12 07:00:00", "2018-12-31")]
        [TestCase(null, null)]
        [TestCase("(null)", null)]
        public void Execute_DateTimeToLastOfYear_Valid(object value, DateTime expected)
        {
            var code = "dateTime-to-last-of-year";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            if (expected == new DateTime(1, 1, 1))
                Assert.That(result, Is.Null);
            else
                Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Execute_NotInitialized_InvalidOperation()
        {
            var provider = new NativeTransformer<string>();

            Assert.Throws<InvalidOperationException>(delegate { provider.Execute(200); });
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(10.566, 10.566)]
        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("(null)", 0)]
        public void Execute_NullToZero_Valid(object value, decimal expected)
        {
            var code = "null-to-zero";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.1, 11)]
        [TestCase(11, 11)]
        [TestCase(10.5, 11)]
        [TestCase(10.7, 11)]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("(null)", null)]
        public void Execute_NumericToCeiling_Valid(object value, decimal expected)
        {
            var code = "numeric-to-ceiling";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            if (expected == 0)
                Assert.That(result, Is.Null);
            else
                Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.1, 10)]
        [TestCase(11, 11)]
        [TestCase(10.5, 10)]
        [TestCase(10.7, 10)]
        public void Execute_NumericToFloor_Valid(object value, decimal expected)
        {
            var code = "numeric-to-floor";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.1, 10)]
        [TestCase(11, 11)]
        [TestCase(10.5, 10)]
        [TestCase(10.7, 11)]
        public void Execute_NumericToInteger_Valid(object value, decimal expected)
        {
            var code = "numeric-to-integer";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.158, 10.16)]
        [TestCase(11, 11)]
        [TestCase(10.153, 10.15)]
        public void Execute_NumericToRound_Valid(object value, decimal expected)
        {
            var code = "numeric-to-round(2)";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10, 8, 12, 10)]
        [TestCase(10, 12, 16, 12)]
        [TestCase(10, 6, 9, 9)]
        public void Execute_NumericToClip_Valid(object value, object min, object max, decimal expected)
        {
            var code = $"numeric-to-clip({min}, {max})";
            var provider = new NativeTransformer<decimal>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2019-03-11", "2019-03-11")]
        [TestCase("2019-02-11", "2019-03-01")]
        [TestCase("2019-04-11", "2019-03-31")]
        public void Execute_DateTimeToClip_Valid(object value, DateTime expected)
        {
            var code = $"dateTime-to-clip(2019-03-01, 2019-03-31)";
            var provider = new NativeTransformer<DateTime>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
