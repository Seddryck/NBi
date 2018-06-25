using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Mutation.ColumnBased;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Mutation.ColumnBased
{

    public class FiterIdentificationTest
    {
        [TestCase("#0")]
        [TestCase("Foo")]
        [Test]
        public void Execute_Identifier_ColumnFilterped(string identifier)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);

            var filter = new FilterIdentification(new[] { id });
            filter.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(1));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Foo"));
        }

        [TestCase("#0", "#2")]
        [TestCase("Foo", "Bar")]
        [Test]
        public void Execute_MultipleIdentifiers_ColumnFilterped(string id1, string id2)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Bar";

            var factory = new ColumnIdentifierFactory();

            var filter = new FilterIdentification(new[] { factory.Instantiate(id1), factory.Instantiate(id2) });
            filter.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Foo"));
            Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Bar"));
        }

        [TestCase("#0", "#0")]
        [TestCase("Foo", "Foo")]
        [TestCase("Foo", "#0")]
        [Test]
        public void Execute_DuplicatedIdentifiers_ColumnFilterped(string id1, string id2)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();

            var filter = new FilterIdentification(new[] { factory.Instantiate(id1), factory.Instantiate(id2) });
            filter.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(1));
        }

        [TestCase("#999")]
        [TestCase("Bar")]
        [Test]
        public void Execute_NonExistingIdentifiers_ColumnFilterped(string id)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();

            var filter = new FilterIdentification(new[] { factory.Instantiate(id) });
            filter.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(0));
        }

    }
}
