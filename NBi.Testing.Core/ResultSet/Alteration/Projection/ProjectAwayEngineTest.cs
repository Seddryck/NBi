using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Projection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Alteration.Projection
{
    public class ProjectAwayEngineTest
    {
        [TestCase("#0")]
        [TestCase("Foo")]
        [Test]
        public void Execute_Identifier_ColumnSkipped(string identifier)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();
            var id = factory.Instantiate(identifier);

            var skip = new ProjectAwayEngine(new ProjectAwayArgs(new[] { id }));
            skip.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col1"));
            Assert.That(rs.Columns[1].ColumnName, Is.EqualTo("Col2"));
        }

        [TestCase("#0", "#2")]
        [TestCase("Foo", "Bar")]
        [Test]
        public void Execute_MultipleIdentifiers_ColumnSkipped(string id1, string id2)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Bar";

            var factory = new ColumnIdentifierFactory();

            var skip = new ProjectAwayEngine(new ProjectAwayArgs(new[] { factory.Instantiate(id1), factory.Instantiate(id2) }));
            skip.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(1));
            Assert.That(rs.Columns[0].ColumnName, Is.EqualTo("Col1"));
        }

        [TestCase("#0", "#0")]
        [TestCase("Foo", "Foo")]
        [TestCase("Foo", "#0")]
        [Test]
        public void Execute_DuplicatedIdentifiers_ColumnSkipped(string id1, string id2)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();

            var skip = new ProjectAwayEngine(new ProjectAwayArgs(new[] { factory.Instantiate(id1), factory.Instantiate(id2) }));
            skip.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
        }

        [TestCase("#999")]
        [TestCase("Bar")]
        [Test]
        public void Execute_NonExistingIdentifiers_ColumnSkipped(string id)
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1;120");
            rs.Columns[0].ColumnName = "Foo";
            rs.Columns[1].ColumnName = "Col1";
            rs.Columns[2].ColumnName = "Col2";

            var factory = new ColumnIdentifierFactory();

            var skip = new ProjectAwayEngine(new ProjectAwayArgs(new[] { factory.Instantiate(id) }));
            skip.Execute(rs);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
        }
    }
}
