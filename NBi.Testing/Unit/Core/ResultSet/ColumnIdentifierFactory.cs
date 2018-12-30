using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet
{
    
    public class ColumnIdentifierFactoryTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(20)]
        [Test]
        public void Instantiate_Ordinal_PositionIdentifier(int identifier)
        {
            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate("#" + identifier.ToString());
            Assert.That(id, Is.TypeOf<ColumnOrdinalIdentifier>());
            Assert.That((id as ColumnOrdinalIdentifier).Ordinal, Is.EqualTo(identifier));
        }

        [Test]
        [TestCase("#0.1")]
        [TestCase("#-1")]
        [TestCase("#Alpha")]
        public void Instantiate_Ordinal_Failure(string identifier)
        {
            var factory = new ColumnIdentifierFactory();
            Assert.Throws<ArgumentException>(() => factory.Instantiate(identifier));
        }

        [Test]
        [TestCase("Foo")]
        [TestCase("-bar")]
        public void Instantiate_Name_NameIdentifier(string identifier)
        {
            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);
            Assert.That(id, Is.TypeOf<ColumnNameIdentifier>());
            Assert.That((id as ColumnNameIdentifier).Name, Is.EqualTo(identifier));
        }

        [Test]
        [TestCase("[Foo]")]
        [TestCase(" [Foo] ")]
        public void Instantiate_NameWithBrackets_NameIdentifier(string identifier)
        {
            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);
            Assert.That(id, Is.TypeOf<ColumnNameIdentifier>());
            Assert.That((id as ColumnNameIdentifier).Name, Is.EqualTo("Foo"));
        }


        [Test]
        [TestCase("[Measures].[Foo]")]
        [TestCase("[dimension].[hierarchy].[level]")]
        public void Instantiate_SsasName_NameIdentifier(string identifier)
        {
            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);
            Assert.That(id, Is.TypeOf<ColumnNameIdentifier>());
            Assert.That((id as ColumnNameIdentifier).Name, Is.EqualTo(identifier));
        }

        [Test]
        public void Instantiate_SsasNameWithDoubleBrackets_NameIdentifier()
        {
            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate("[[dimension].[hierarchy].[level]]");
            Assert.That(id, Is.TypeOf<ColumnNameIdentifier>());
            Assert.That((id as ColumnNameIdentifier).Name, Is.EqualTo("[dimension].[hierarchy].[level]"));
        }
    }
}
