using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Projection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Projection
{
    public class ProjectEngineTest
    {
        [TestCase("#0")]
        [TestCase("Foo")]
        [Test]
        public void Execute_Identifier_ColumnFilterped(string identifier)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Foo");
            rs.GetColumn(1)?.Rename("Col1");
            rs.GetColumn(2)?.Rename("Col2");

            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);

            var filter = new ProjectEngine(new ProjectArgs([id]));
            filter.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(1));
            Assert.That(rs.GetColumn(0)?.Name, Is.EqualTo("Foo"));
        }

        [TestCase("#0", "#2")]
        [TestCase("Foo", "Bar")]
        [Test]
        public void Execute_MultipleIdentifiers_ColumnFilterped(string id1, string id2)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Foo");
            rs.GetColumn(1)?.Rename("Col1");
            rs.GetColumn(2)?.Rename("Bar");

            var factory = new ColumnIdentifierFactory();

            var filter = new ProjectEngine(new ProjectArgs([factory.Instantiate(id1), factory.Instantiate(id2)]));
            filter.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs.GetColumn(0)?.Name, Is.EqualTo("Foo"));
            Assert.That(rs.GetColumn(1)?.Name, Is.EqualTo("Bar"));
        }

        [TestCase("#2", "#0")]
        [TestCase("Bar", "Foo")]
        [Test]
        public void Execute_MultipleIdentifiersNotSameOrder_ColumnFilteredOrderChanged(string id1, string id2)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Foo");
            rs.GetColumn(1)?.Rename("Col1");
            rs.GetColumn(2)?.Rename("Bar");

            var factory = new ColumnIdentifierFactory();

            var project = new ProjectEngine(new ProjectArgs([factory.Instantiate(id1), factory.Instantiate(id2)]));
            project.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs.GetColumn(0)?.Name, Is.EqualTo("Bar"));
            Assert.That(rs.GetColumn(1)?.Name, Is.EqualTo("Foo"));
        }

        [TestCase("#0", "#0")]
        [TestCase("Foo", "Foo")]
        [TestCase("Foo", "#0")]
        [Test]
        public void Execute_DuplicatedIdentifiers_ColumnFilterped(string id1, string id2)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Foo");
            rs.GetColumn(1)?.Rename("Col1");
            rs.GetColumn(2)?.Rename("Col2");

            var factory = new ColumnIdentifierFactory();

            var filter = new ProjectEngine(new ProjectArgs([factory.Instantiate(id1), factory.Instantiate(id2)]));
            filter.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(1));
        }

        [TestCase("#1", "#1")]
        [TestCase("Foo", "Foo")]
        [TestCase("Foo", "#1")]
        [TestCase("#1", "Bar")]
        [Test]
        public void Execute_DuplicatedIdentifiersAndChangeOrder_ColumnFilteredOrderedChanged(string id1, string id2)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Col1");
            rs.GetColumn(1)?.Rename("Foo");
            rs.GetColumn(2)?.Rename("Bar");

            var factory = new ColumnIdentifierFactory();

            var filter = new ProjectEngine(new ProjectArgs([factory.Instantiate("#2"), factory.Instantiate(id1), factory.Instantiate(id2)]));
            filter.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs.GetColumn(0)?.Name, Is.EqualTo("Bar"));
            Assert.That(rs.GetColumn(1)?.Name, Is.EqualTo("Foo"));
        }

        [TestCase("#999")]
        [TestCase("Bar")]
        [Test]
        public void Execute_NonExistingIdentifiers_ColumnFilterped(string id)
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1;120");
            rs.GetColumn(0)?.Rename("Foo");
            rs.GetColumn(1)?.Rename("Col1");
            rs.GetColumn(2)?.Rename("Col2");

            var factory = new ColumnIdentifierFactory();

            var filter = new ProjectEngine(new ProjectArgs([factory.Instantiate(id)]));
            filter.Execute(rs);

            Assert.That(rs.ColumnCount, Is.EqualTo(0));
        }
    }
}
