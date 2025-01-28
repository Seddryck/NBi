using NBi.Core.ResultSet.Alteration.Reshaping;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Diagnostics;

namespace NBi.Core.Testing.ResultSet.Alteration.Reshaping;

public class UnstackEngineTest
{
    [Test]
    public void Execute_SingleKeySingleValue_ExpectedResultSet()
    {
        var resolver = new ObjectsResultSetResolver(
            new ObjectsResultSetResolverArgs(
                new[] {
                    ["alpha", "A", 1],
                    ["alpha", "B", 2],
                    ["beta", "A", 3],
                    new object[] { "beta", "B", 4 }
                }
            ));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("keyColumn");
        rs.GetColumn(1)?.Rename("headerColumn");
        rs.GetColumn(2)?.Rename("valueColumn");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text)]
            );

        var unstack = new UnstackEngine(args);
        var result = unstack.Execute(rs);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ColumnCount, Is.EqualTo(3));
        Assert.That(result.Columns.Any(x => x.Name == "keyColumn"));
        Assert.That(result.Columns.Any(x => x.Name == "A"));
        Assert.That(result.Columns.Any(x => x.Name == "B"));
        Assert.That(result.Rows.Count, Is.EqualTo(2));
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["A"]) == 1);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["B"]) == 2);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["A"]) == 3);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["B"]) == 4);
    }

    [Test]
    public void Execute_SingleKeyMultipleValue_ExpectedResultSet()
    {
        var resolver = new ObjectsResultSetResolver(
            new ObjectsResultSetResolverArgs(
                new[] {
                    ["alpha", "A", 1, -1],
                    new object[] { "alpha", "B", 2, -2 },
                    ["beta", "A", 3, -3],
                    ["beta", "B", 4, -4]
                }
            ));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("keyColumn");
        rs.GetColumn(1)?.Rename("headerColumn");
        rs.GetColumn(2)?.Rename("value1Column");
        rs.GetColumn(3)?.Rename("value2Column");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text)]
            );

        var unstack = new UnstackEngine(args);
        var result = unstack.Execute(rs);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ColumnCount, Is.EqualTo(5));
        Assert.That(result.Columns.Any(x => x.Name == "keyColumn"));
        Assert.That(result.Columns.Any(x => x.Name == "A_value1Column"));
        Assert.That(result.Columns.Any(x => x.Name == "B_value1Column"));
        Assert.That(result.Columns.Any(x => x.Name == "A_value2Column"));
        Assert.That(result.Columns.Any(x => x.Name == "B_value2Column"));
        Assert.That(result.Rows.Count, Is.EqualTo(2));
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["A_value1Column"]) == 1);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["B_value1Column"]) == 2);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["A_value1Column"]) == 3);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["B_value1Column"]) == 4);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["A_value2Column"]) == -1);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["B_value2Column"]) == -2);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["A_value2Column"]) == -3);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["keyColumn"] as string == "beta")["B_value2Column"]) == -4);
    }

    [Test]
    public void Execute_MultipleKeysSingleValue_ExpectedResultSet()
    {
        var resolver = new ObjectsResultSetResolver(
            new ObjectsResultSetResolverArgs(
                new[] {
                    new object[] { "alpha", "one", "A", 1 },
                    ["alpha", "one", "B", 2],
                    ["beta", "one", "A", 3],
                    ["beta", "one", "B", 4],
                    ["beta", "two", "B", -4]
                }
            ));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("key1Column");
        rs.GetColumn(1)?.Rename("key2Column");
        rs.GetColumn(2)?.Rename("headerColumn");
        rs.GetColumn(3)?.Rename("valueColumn");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [
                    Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("key1Column") && x.Type == ColumnType.Text),
                    Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("key2Column") && x.Type == ColumnType.Text),
                ]
            );

        var unstack = new UnstackEngine(args);
        var result = unstack.Execute(rs);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ColumnCount, Is.EqualTo(4));
        Assert.That(result.Columns.Any(x => x.Name == "key1Column"));
        Assert.That(result.Columns.Any(x => x.Name == "key2Column"));
        Assert.That(result.Columns.Any(x => x.Name == "A"));
        Assert.That(result.Columns.Any(x => x.Name == "B"));
        Assert.That(result.Rows.Count, Is.EqualTo(3));
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["key1Column"] as string == "alpha" && x["key2Column"] as string == "one")["A"]) == 1);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["key1Column"] as string == "alpha" && x["key2Column"] as string == "one")["B"]) == 2);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "one")["A"]) == 3);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "one")["B"]) == 4);
        Assert.That(Convert.ToInt32(result.Rows.Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "two")["B"]) == -4);
        Assert.That(result.Rows.Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "two")["A"] == DBNull.Value);
    }

    [Test]
    public void Execute_EnforcedColumnsThatWasNotExpected_ExpectedResultSet()
    {
        var resolver = new ObjectsResultSetResolver(
            new ObjectsResultSetResolverArgs(
                new[] {
                    new object[] { "alpha", "A", 1 },
                    ["alpha", "B", 2],
                    ["beta", "A", 3],
                    ["beta", "B", 4 ]
                }
            ));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("keyColumn");
        rs.GetColumn(1)?.Rename("headerColumn");
        rs.GetColumn(2)?.Rename("valueColumn");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text)],
                [new ColumnNameIdentifier("C")]
            );

        var unstack = new UnstackEngine(args);
        var result = unstack.Execute(rs);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ColumnCount, Is.EqualTo(4));
        Assert.That(result.Columns.Any(x => x.Name == "keyColumn"));
        Assert.That(result.Columns.Any(x => x.Name == "A"));
        Assert.That(result.Columns.Any(x => x.Name == "B"));
        Assert.That(result.Columns.Any(x => x.Name == "C"));
        Assert.That(result.Rows.Count, Is.EqualTo(2));
        Assert.That(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["C"] == DBNull.Value);
        Assert.That(result.Rows.Single(x => x["keyColumn"] as string == "alpha")["C"] == DBNull.Value);
    }

    [Test]
    public void Execute_EnforcedColumnsThatWasAlreadyExpected_ExpectedResultSet()
    {
        var resolver = new ObjectsResultSetResolver(
            new ObjectsResultSetResolverArgs(
                new[] {
                    new object[] { "alpha", "A", 1 },
                    ["alpha", "B", 2],
                    ["beta", "A", 3 ],
                    ["beta", "B", 4 ]
                }
            ));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("keyColumn");
        rs.GetColumn(1)?.Rename("headerColumn");
        rs.GetColumn(2)?.Rename("valueColumn");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text)],
                [new ColumnNameIdentifier("A"), new ColumnNameIdentifier("B")]
            );

        var unstack = new UnstackEngine(args);
        var result = unstack.Execute(rs);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ColumnCount, Is.EqualTo(3));
        Assert.That(result.Columns.Any(x => x.Name == "keyColumn"));
        Assert.That(result.Columns.Any(x => x.Name == "A"));
        Assert.That(result.Columns.Any(x => x.Name == "B"));
        Assert.That(result.Columns.Count(x => x.Name == "A") == 1);
        Assert.That(result.Columns.Count(x => x.Name == "B") == 1);
    }

    [Test]
    [TestCase(100)]
    [TestCase(1000)]
    [TestCase(10000)]
    [TestCase(40000)]
    [Retry(3)]
    [Parallelizable(ParallelScope.Self)]
    public void Execute_LargeResultSet_ExpectedPerformance(int count)
    {
        var values = new List<object>();
        for (int i = 0; i < count; i++)
            values.Add(new object[] { i % 2 == 0 ? "alpha" : "beta", (i/2) % (count/2), 1 });

        var resolver = new ObjectsResultSetResolver(new ObjectsResultSetResolverArgs(values));
        var rs = resolver.Execute();
        rs.GetColumn(0)?.Rename("keyColumn");
        rs.GetColumn(1)?.Rename("headerColumn");
        rs.GetColumn(2)?.Rename("valueColumn");

        var args = new UnstackArgs(
                new ColumnNameIdentifier("headerColumn"),
                [Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text)]
            );

        var unstack = new UnstackEngine(args);

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = unstack.Execute(rs);
        stopWatch.Stop();
        Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(20));
    }
}
