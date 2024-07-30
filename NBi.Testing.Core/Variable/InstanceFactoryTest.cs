using Moq;
using NBi.Core.Injection;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using NBi.Core.Variable.Instantiation;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Variable
{
    public class InstanceFactoryTest
    {
        [Test]
        [Culture("en-us")]
        public void Instantiate_DerivedFromMain_Success()
        {
            var resolver = new Mock<ISequenceResolver>();
            resolver.Setup(x => x.Execute()).Returns(new[] { "BE_20190101", "BE_20190102", "BE_20190103", "BE_20190104", "BE_20190105" });

            var firstTransformation = new NativeTransformer<string>(new ServiceLocator(), Context.None);;
            firstTransformation.Initialize("text-to-first-chars(2)");

            var secondTransformation = new NativeTransformer<string>(new ServiceLocator(), Context.None);;
            secondTransformation.Initialize("text-to-last-chars(8) | text-to-dateTime(yyyyMMdd)");

            var thirdTransformation = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);;
            thirdTransformation.Initialize("dateTime-to-add(7)");

            var args = new DerivedVariableInstanceArgs
            (
                "main",
                resolver.Object,
                new Dictionary<string, DerivationArgs>()
                {
                    { "first", new DerivationArgs("main", firstTransformation) },
                    { "second", new DerivationArgs("main", secondTransformation) },
                    { "third", new DerivationArgs("second", thirdTransformation) }
                }
            );

            var factory = new InstanceFactory();
            var instances = factory.Instantiate(args);
            Assert.That(instances.Count, Is.EqualTo(5));
            Assert.That(instances.ElementAt(0).Variables.Count, Is.EqualTo(4));
            Assert.That(instances.ElementAt(0).Variables.ContainsKey("main"), Is.True);
            Assert.That(instances.ElementAt(0).Variables["main"].GetValue(), Is.EqualTo("BE_20190101"));
            Assert.That(instances.ElementAt(0).Variables.ContainsKey("first"), Is.True);
            Assert.That(instances.ElementAt(0).Variables["first"].GetValue(), Is.EqualTo("BE"));
            Assert.That(instances.ElementAt(0).Variables.ContainsKey("second"), Is.True);
            Assert.That(instances.ElementAt(0).Variables["second"].GetValue(), Is.EqualTo(new DateTime(2019, 1, 1)));
            Assert.That(instances.ElementAt(0).Variables.ContainsKey("third"), Is.True);
            Assert.That(instances.ElementAt(0).Variables["third"].GetValue(), Is.EqualTo(new DateTime(2019, 1, 8)));

            Assert.That(instances.ElementAt(1).Variables.Count, Is.EqualTo(4));
            Assert.That(instances.ElementAt(1).Variables.ContainsKey("main"), Is.True);
            Assert.That(instances.ElementAt(1).Variables["main"].GetValue(), Is.EqualTo("BE_20190102"));
            Assert.That(instances.ElementAt(1).Variables.ContainsKey("first"), Is.True);
            Assert.That(instances.ElementAt(1).Variables["first"].GetValue(), Is.EqualTo("BE"));
            Assert.That(instances.ElementAt(1).Variables.ContainsKey("second"), Is.True);
            Assert.That(instances.ElementAt(1).Variables["second"].GetValue(), Is.EqualTo(new DateTime(2019, 1, 2)));
            Assert.That(instances.ElementAt(1).Variables.ContainsKey("third"), Is.True);
            Assert.That(instances.ElementAt(1).Variables["third"].GetValue(), Is.EqualTo(new DateTime(2019, 1, 9)));
        }
    }
}