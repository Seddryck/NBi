using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Extension
{
    public class ExtensionFactoryTest
    {
        [Test]
        public void Instantiate_ExtendArgsNCalc_NCalcExtendEngine()
        {
            var factory = new ExtensionFactory(Core.Injection.ServiceLocator.None, Context.None);
            var extender = factory.Instantiate(new ExtendArgs(
                new ColumnOrdinalIdentifier(1),
                "a+b*c",
                LanguageType.NCalc
                ));
            Assert.That(extender, Is.Not.Null);
            Assert.That(extender, Is.TypeOf<NCalcExtendEngine>());
        }

        [Test]
        public void Instantiate_ExtendArgsNative_NativeExtendEngine()
        {
            var factory = new ExtensionFactory(Core.Injection.ServiceLocator.None, Context.None);
            var extender = factory.Instantiate(new ExtendArgs(
                new ColumnOrdinalIdentifier(1),
                "[A] | dateTime-to-date | dateTime-to-add(00:15:00, [B])",
                LanguageType.Native
                ));
            Assert.That(extender, Is.Not.Null);
            Assert.That(extender, Is.TypeOf<NativeExtendEngine>());
        }
    }
}
