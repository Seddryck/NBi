using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Transformation.Transformer
{
    public class NativeTransformationFactoryTest
    {
        [Test]
        [TestCase("length")]
        [TestCase("length()")]
        [TestCase("\t\rlength \n")]
        [TestCase("\t\rlength \n()")]
        public void Instantiate_WhitespaceIrrelevant_CorrectlyBuilt(string value)
        {
            var factory = new NativeTransformationFactory(new ServiceLocator(), Context.None); ;
            var result = factory.Instantiate(value);

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
        }

        [Test]
        [TestCase("blank-to-empty")]
        [TestCase("blank-to-null")]
        [TestCase("empty-to-null")]
        [TestCase("null-to-empty")]
        [TestCase("null-to-value")]
        [TestCase("null-to-zero")]
        [TestCase("value-to-value")]
        [TestCase("any-to-any")]
        [TestCase("text-to-trim")]
        [TestCase("text-to-upper")]
        [TestCase("text-to-lower")]
        [TestCase("text-to-suffix(abc)")]
        [TestCase("text-to-prefix(abc)")]
        [TestCase("text-to-length")]
        [TestCase("text-to-first-chars(3)")]
        [TestCase("text-to-last-chars(3)")]
        [TestCase("text-to-skip-first-chars(3)")]
        [TestCase("text-to-skip-last-chars(3)")]
        [TestCase("text-to-pad-right(3, *)")]
        [TestCase("text-to-pad-left(3, *)")]
        [TestCase("text-to-html")]
        [TestCase("text-to-without-diacritics")]
        [TestCase("text-to-token-count")]
        [TestCase("text-to-token-count(;)")]
        [TestCase("text-to-token(2)")]
        [TestCase("text-to-token(2,;)")]
        [TestCase("text-to-without-whitespaces")]
        [TestCase("text-to-remove-chars(*)")]
        [TestCase("text-to-dateTime(\"yyyy.mm.dd hh:mm\")")]
        [TestCase("text-to-dateTime(\"dddd dd mm yyyy hh:mm\", fr-fr)")]
        [TestCase("text-to-mask(BE-***.***.**)")]
        [TestCase("mask-to-text(BE-***.***.**)")]
        [TestCase("html-to-text")]
        [TestCase("date-to-age")]
        [TestCase("utc-to-local(Brussels)")]
        [TestCase("utc-to-local(\"Romance Standard Time\")")]
        [TestCase("local-to-utc(Brussels)")]
        [TestCase("dateTime-to-date")]
        [TestCase("null-to-date(2010-05-01)")]
        [TestCase("dateTime-to-first-of-month")]
        [TestCase("dateTime-to-last-of-month")]
        [TestCase("dateTime-to-first-of-year")]
        [TestCase("dateTime-to-last-of-year")]
        [TestCase("dateTime-to-next-day")]
        [TestCase("dateTime-to-previous-day")]
        [TestCase("dateTime-to-next-month")]
        [TestCase("dateTime-to-previous-month")]
        [TestCase("dateTime-to-next-year")]
        [TestCase("dateTime-to-previous-year")]
        [TestCase("dateTime-to-floor-hour")]
        [TestCase("dateTime-to-ceiling-hour")]
        [TestCase("dateTime-to-floor-minute")]
        [TestCase("dateTime-to-ceiling-minute")]
        [TestCase("dateTime-to-clip(2019-01-01, 2019-12-31)")]
        [TestCase("dateTime-to-set-time(07:00:00)")]
        [TestCase("dateTime-to-add(00:15:00)")]
        [TestCase("dateTime-to-add(00:15:00, 3)")]
        [TestCase("dateTime-to-subtract(00:15:00)")]
        [TestCase("dateTime-to-subtract(00:15:00, 3)")]
        [TestCase("numeric-to-round(5)")]
        [TestCase("numeric-to-floor")]
        [TestCase("numeric-to-ceiling")]
        [TestCase("numeric-to-integer")]
        [TestCase("numeric-to-increment")]
        [TestCase("numeric-to-decrement")]
        [TestCase("numeric-to-clip(10, 20)")]
        [TestCase("numeric-to-add(10)")]
        [TestCase("numeric-to-add(10, 3)")]
        [TestCase("numeric-to-subtract(10)")]
        [TestCase("numeric-to-subtract(10, 3)")]
        [TestCase("numeric-to-multiply(10)")]
        [TestCase("numeric-to-divide(12)")]
        [TestCase("numeric-to-invert")]
        public void Instantiate_ExistingNativeTransformation_CorrectlyBuilt(string value)
        {
            var factory = new NativeTransformationFactory(new ServiceLocator(), Context.None);;
            var result = factory.Instantiate(value);

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
        }

        [Test]
        [TestCase("path-to-filename")]
        [TestCase("path-to-filename-without-extension")]
        [TestCase("path-to-extension")]
        [TestCase("path-to-root")]
        [TestCase("path-to-directory")]
        [TestCase("file-to-size")]
        [TestCase("file-to-creation-dateTime")]
        [TestCase("file-to-creation-dateTime-utc")]
        [TestCase("file-to-update-dateTime")]
        [TestCase("file-to-update-dateTime-utc")]
        public void Instantiate_ExistingNativeTransformationWithFilePath_CorrectlyBuilt(string value)
        {
            //value = $"{value}({GetType().Assembly.Location})";
            var factory = new NativeTransformationFactory(new ServiceLocator(), Context.None); ;
            var result = factory.Instantiate(value);

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
        }
    }
}
