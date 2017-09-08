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
            var code = "trim";
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
            var code = "trim";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.Not.EqualTo("ABC"));
        }


        [Test]
        [TestCase("abC")]
        public void Execute_UpperCase_ABC(object value)
        {
            var code = "upper-case";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo("ABC"));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_LowerCase_abc(object value)
        {
            var code = "lower-case";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(" abc "));
        }

        [Test]
        [TestCase(" abC ")]
        public void Execute_Length_5(object value)
        {
            var code = "length";
            var provider = new NativeTransformer<string>();
            provider.Initialize(code);

            var result = provider.Execute(value);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
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
        public void Execute_NotInitialized_InvalidOperation()
        {
            var provider = new NativeTransformer<string>();

            Assert.Throws<InvalidOperationException>(delegate { provider.Execute(200); });
        }
    }
}
