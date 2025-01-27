using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Calculation.Ranking;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Calculation.Ranking;

public class TopRankingTest
{
    [Test]
    [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, 4)]
    [TestCase(new object[] { "a", "b", "e", "c", "d" }, ColumnType.Text, 3)]
    [TestCase(new object[] { "2010-02-02 07:12:16.52", "2010-02-02 07:12:16.55", "2010-02-02 08:12:16.50" }, ColumnType.DateTime, 3)]

    public void Apply_Rows_Success(object[] values, ColumnType columnType, int index)
    {
        var i = 0;
        var objs = values.Select(x => new object[] { ++i, x }).ToArray();

        var args = new ObjectsResultSetResolverArgs(objs);
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var ranking = new TopRanking(new ColumnOrdinalIdentifier(1), columnType, [], []);
        var filteredRs = ranking.Apply(rs);

        Assert.That(filteredRs.RowCount, Is.EqualTo(1));
        Assert.That(filteredRs[0][0], Is.EqualTo(index));
        Assert.That(filteredRs[0][1], Is.EqualTo(values.Max()));
    }

    [Test]
    [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, new int[] { 4, 2 })]
    [TestCase(new object[] { "a", "b", "e", "c", "d" }, ColumnType.Text, new int[] { 3, 5 })]
    [TestCase(new object[] { "2010-02-02 07:12:16.52", "2010-02-02 07:12:16.55", "2010-02-02 08:12:16.50" }, ColumnType.DateTime, new int[] { 3, 2 })]
    public void Apply_TopTwo_Success(object[] values, ColumnType columnType, int[] index)
    {
        var i = 0;
        var objs = values.Select(x => new object[] { ++i, x }).ToArray();

        var args = new ObjectsResultSetResolverArgs(objs);
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var ranking = new TopRanking(2, new ColumnOrdinalIdentifier(1), columnType, [], []);
        var filteredRs = ranking.Apply(rs);

        Assert.That(filteredRs.RowCount, Is.EqualTo(2));
        Assert.That(filteredRs[0][0], Is.EqualTo(index[0]));
        Assert.That(filteredRs[0][1], Is.EqualTo(values.Max()));
        Assert.That(filteredRs[1][0], Is.EqualTo(index[1]));
        Assert.That(filteredRs[1][1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Max(), 1)).Max()));
    }

    [Test]
    [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, new int[] { 4, 2, 1 })]
    public void Apply_Larger_Success(object[] values, ColumnType columnType, int[] index)
    {
        var i = 0;
        var objs = values.Select(x => new object[] { ++i, x }).ToArray();

        var args = new ObjectsResultSetResolverArgs(objs);
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var ranking = new TopRanking(10, new ColumnOrdinalIdentifier(1), columnType, [], []);
        var filteredRs = ranking.Apply(rs);

        Assert.That(filteredRs.RowCount, Is.EqualTo(values.Length));
        Assert.That(filteredRs[0][0], Is.EqualTo(index[0]));
        Assert.That(filteredRs[0][1], Is.EqualTo(values.Max()));
        Assert.That(filteredRs[1][0], Is.EqualTo(index[1]));
        Assert.That(filteredRs[1][1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Max(), 1)).Max()));
        Assert.That(filteredRs[values.Length - 1][0], Is.EqualTo(index[2]));
        Assert.That(filteredRs[values.Length - 1][1], Is.EqualTo(values.Min()));
    }


    [Test]
    [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, 4)]
    public void Apply_Alias_Success(object[] values, ColumnType columnType, int index)
    {
        var i = 0;
        var objs = values.Select(x => new object[] { ++i, x }).ToArray();

        var args = new ObjectsResultSetResolverArgs(objs);
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var alias = Mock.Of<IColumnAlias>(x => x.Column == 1 && x.Name == "myValue");

        var ranking = new TopRanking(new ColumnNameIdentifier("myValue"), columnType, Enumerable.Repeat(alias, 1), []);
        var filteredRs = ranking.Apply(rs);

        Assert.That(filteredRs.RowCount, Is.EqualTo(1));
        Assert.That(filteredRs[0][0], Is.EqualTo(index));
        Assert.That(filteredRs[0][1], Is.EqualTo(values.Max()));
    }

    [Test]
    [TestCase(new object[] { "108", "128", "118", "139", "125" }, ColumnType.Numeric, 4)]
    public void Apply_Exp_Success(object[] values, ColumnType columnType, int index)
    {
        var i = 0;
        var objs = values.Select(x => new object[] { ++i, x }).ToArray();

        var args = new ObjectsResultSetResolverArgs(objs);
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var alias = Mock.Of<IColumnAlias>(x => x.Column == 1 && x.Name == "myValue");
        var exp = Mock.Of<IColumnExpression>(x => x.Name == "exp" && x.Value == "myValue % 10");

        var ranking = new TopRanking(new ColumnNameIdentifier("exp"), columnType, Enumerable.Repeat(alias, 1), Enumerable.Repeat(exp, 1));
        var filteredRs = ranking.Apply(rs);

        Assert.That(filteredRs.RowCount, Is.EqualTo(1));
        Assert.That(filteredRs[0][0], Is.EqualTo(index));
        Assert.That(filteredRs[0][1], Is.EqualTo("139"));
    }

}
