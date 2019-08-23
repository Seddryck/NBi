using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Extension;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Alteration.Extension
{
    public class ExtensionFactoryTest
    {
        [Test]
        public void Instantiate_ExtendArgs_ExtendEngine()
        {
            var factory = new ExtensionFactory();
            var extender = factory.Instantiate(new ExtendArgs(
                new ColumnOrdinalIdentifier(1),
                "a+b*c"
                ));
            Assert.That(extender, Is.Not.Null);
            Assert.That(extender, Is.TypeOf<ExtendEngine>());
        }
    }
}
