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
    }
}
