using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Projection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Projection;

public class ProjectAwayEngineTest
{
    [TestCase("#0")]
    [TestCase("Foo")]
    [Test]
    public void Execute_Identifier_ColumnSkipped(string identifier)
    {
        var rs = new DataTableResultSet();
        rs.Load("a;1;120");
        rs.GetColumn(0)?.Rename("Foo");
        rs.GetColumn(1)?.Rename("Col1");
        rs.GetColumn(2)?.Rename("Col2");

        var factory = new ColumnIdentifierFactory();
        var id = factory.Instantiate(identifier);

        var skip = new ProjectAwayEngine(new ProjectAwayArgs([id]));
        skip.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(2));
        Assert.That(rs?.GetColumn(0)?.Name, Is.EqualTo("Col1"));
        Assert.That(rs?.GetColumn(1)?.Name, Is.EqualTo("Col2"));
    }

    [TestCase("#0", "#2")]
    [TestCase("Foo", "Bar")]
    [Test]
    public void Execute_MultipleIdentifiers_ColumnSkipped(string id1, string id2)
    {
        var rs = new DataTableResultSet();
        rs.Load("a;1;120");
        rs.GetColumn(0)?.Rename("Foo");
        rs.GetColumn(1)?.Rename("Col1");
        rs.GetColumn(2)?.Rename("Bar");

        var factory = new ColumnIdentifierFactory();

        var skip = new ProjectAwayEngine(new ProjectAwayArgs([factory.Instantiate(id1), factory.Instantiate(id2)]));
        skip.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(1));
        Assert.That(rs?.GetColumn(0)?.Name, Is.EqualTo("Col1"));
    }

    [TestCase("#0", "#0")]
    [TestCase("Foo", "Foo")]
    [TestCase("Foo", "#0")]
    [Test]
    public void Execute_DuplicatedIdentifiers_ColumnSkipped(string id1, string id2)
    {
        var rs = new DataTableResultSet();
        rs.Load("a;1;120");
        rs.GetColumn(0)?.Rename("Foo");
        rs.GetColumn(1)?.Rename("Col1");
        rs.GetColumn(2)?.Rename("Col2");

        var factory = new ColumnIdentifierFactory();

        var skip = new ProjectAwayEngine(new ProjectAwayArgs([factory.Instantiate(id1), factory.Instantiate(id2)]));
        skip.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(2));
    }

    [TestCase("#999")]
    [TestCase("Bar")]
    [Test]
    public void Execute_NonExistingIdentifiers_ColumnSkipped(string id)
    {
        var rs = new DataTableResultSet();
        rs.Load("a;1;120");
        rs.GetColumn(0)?.Rename("Foo");
        rs.GetColumn(1)?.Rename("Col1");
        rs.GetColumn(2)?.Rename("Col2");

        var factory = new ColumnIdentifierFactory();

        var skip = new ProjectAwayEngine(new ProjectAwayArgs([factory.Instantiate(id)]));
        skip.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(3));
    }
}
