using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Transformation.Transformer
{
    public class NativeTransformationFactoryTest
    {
        [Test]
        public void Instantiate_ExistingWithoutParameter_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("dateTime-to-date");
            
            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<DateTimeToDate>());
        }

        [Test]
        public void Instantiate_ExistingWithoutParameterAndWhitespaces_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("\t\tdateTime-to-date\r\n");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<DateTimeToDate>());
        }

        [Test]
        public void Instantiate_ExistingWithParameter_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("utc-to-local(Brussels)");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<UtcToLocal>());
            Assert.That((result as UtcToLocal).TimeZoneLabel, Is.EqualTo("Brussels"));
        }

        [Test]
        public void Instantiate_ExistingWithParameterIncludingSpaces_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("utc-to-local( Romance Standard Time )");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<UtcToLocal>());
            Assert.That((result as UtcToLocal).TimeZoneLabel, Is.EqualTo("Romance Standard Time"));
        }


        [Test]
        public void Instantiate_ExistingWithParameterAndWhitespaces_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("\r\n\t\t\tutc-to-local(Brussels) \t\r\n");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<UtcToLocal>());
            Assert.That((result as UtcToLocal).TimeZoneLabel, Is.EqualTo("Brussels"));
        }

        [Test]
        public void Instantiate_ExistingWithParameters_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("numeric-to-clip(10, 2000)");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<NumericToClip>());
            Assert.That((result as NumericToClip).Min, Is.EqualTo(10));
            Assert.That((result as NumericToClip).Max, Is.EqualTo(2000));
        }

        [Test]
        public void Instantiate_ExistingWithParametersAndWhitespaces_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("\r\n\t\t\tnumeric-to-clip(  10,   2000   )\t\t\t\r\n");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<NumericToClip>());
            Assert.That((result as NumericToClip).Min, Is.EqualTo(10));
            Assert.That((result as NumericToClip).Max, Is.EqualTo(2000));
        }

        [Test]
        public void Instantiate_ExistingWithParametersAndSpaces_CorrectType()
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate("numeric-to-clip (10,   2000)");

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
            Assert.That(result, Is.TypeOf<NumericToClip>());
            Assert.That((result as NumericToClip).Min, Is.EqualTo(10));
            Assert.That((result as NumericToClip).Max, Is.EqualTo(2000));
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
        [TestCase("text-to-length")]
        [TestCase("text-to-html")]
        [TestCase("text-to-without-diacritics")]
        [TestCase("text-to-token-count")]
        [TestCase("text-to-without-whitespaces")]
        [TestCase("html-to-text")]
        [TestCase("date-to-age")]
        [TestCase("utc-to-local(Brussels)")]
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
        [TestCase("dateTime-to-set-time(07:00:00)")]
        [TestCase("numeric-to-round(5)")]
        [TestCase("numeric-to-floor")]
        [TestCase("numeric-to-ceiling")]
        [TestCase("numeric-to-integer")]
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
        public void Instantiate_ExistingNativeTransformation_CorrectlyBuilt(string value)
        {
            var factory = new NativeTransformationFactory();
            var result = factory.Instantiate(value);

            Assert.That(result, Is.AssignableTo<INativeTransformation>());
        }
    }
}
