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

namespace NBi.Core.Testing.Calculation.Ranking
{
    public class BottomRankingTest
    {
        [Test]
        [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, 1)]
        [TestCase(new object[] { "d", "b", "e", "a", "d" }, ColumnType.Text, 4)]
        [TestCase(new object[] { "2010-02-02 07:12:17.52", "2010-02-02 07:12:16.55", "2010-02-02 08:12:16.50" }, ColumnType.DateTime, 2)]
        public void Apply_Rows_Success(object[] values, ColumnType columnType, int index)
        {
            var i = 0;
            var objs = values.Select(x => new object[] { ++i, x }).ToArray();

            var args = new ObjectsResultSetResolverArgs(objs);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var ranking = new BottomRanking(new ColumnOrdinalIdentifier(1), columnType, [], []);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.RowCount, Is.EqualTo(1));
            Assert.That(filteredRs[0][0], Is.EqualTo(index));
            Assert.That(filteredRs[0][1], Is.EqualTo(values.Min()));
        }

        [Test]
        [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, new int[] { 1, 5 })]
        [TestCase(new object[] { "d", "e", "a", "c", "b" }, ColumnType.Text, new int[] { 3, 5 })]
        [TestCase(new object[] { "2010-02-02 07:12:16.52", "2010-02-02 07:12:16.55", "2010-02-02 08:12:16.50" }, ColumnType.DateTime, new int[] { 1, 2 })]
        public void Apply_TopTwo_Success(object[] values, ColumnType columnType, int[] index)
        {
            var i = 0;
            var objs = values.Select(x => new object[] { ++i, x }).ToArray();

            var args = new ObjectsResultSetResolverArgs(objs);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var ranking = new BottomRanking(2, new ColumnOrdinalIdentifier(1), columnType, [], []);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.RowCount, Is.EqualTo(2));
            Assert.That(filteredRs[0][0], Is.EqualTo(index[0]));
            Assert.That(filteredRs[0][1], Is.EqualTo(values.Min()));
            Assert.That(filteredRs[1][0], Is.EqualTo(index[1]));
            Assert.That(filteredRs[1][1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Min(), 1)).Min()));
        }

        [Test]
        [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, new int[] { 1, 5, 4 })]
        public void Apply_Larger_Success(object[] values, ColumnType columnType, int[] index)
        {
            var i = 0;
            var objs = values.Select(x => new object[] { ++i, x }).ToArray();

            var args = new ObjectsResultSetResolverArgs(objs);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var ranking = new BottomRanking(10, new ColumnOrdinalIdentifier(1), columnType, [], []);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.RowCount, Is.EqualTo(values.Count()));
            Assert.That(filteredRs[0][0], Is.EqualTo(index[0]));
            Assert.That(filteredRs[0][1], Is.EqualTo(values.Min()));
            Assert.That(filteredRs[1][0], Is.EqualTo(index[1]));
            Assert.That(filteredRs[1][1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Min(), 1)).Min()));
            Assert.That(filteredRs[values.Count() - 1][0], Is.EqualTo(index[2]));
            Assert.That(filteredRs[values.Count() - 1][1], Is.EqualTo(values.Max()));
        }

        [Test]
        [TestCase(new object[] { "100", "120", "110", "130", "105" }, ColumnType.Numeric, 1)]
        public void Apply_Alias_Success(object[] values, ColumnType columnType, int index)
        { 
            var i = 0;
            var objs = values.Select(x => new object[] { ++i, x }).ToArray();

            var args = new ObjectsResultSetResolverArgs(objs);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var alias = Mock.Of<IColumnAlias>(x => x.Column == 1 && x.Name == "myValue");

            var ranking = new BottomRanking(new ColumnNameIdentifier("myValue"), columnType, Enumerable.Repeat(alias, 1), []);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.RowCount, Is.EqualTo(1));
            Assert.That(filteredRs[0][0], Is.EqualTo(index));
            Assert.That(filteredRs[0][1], Is.EqualTo(values.Min()));
        }

        [Test]
        [TestCase(new object[] { "108", "128", "118", "137", "125" }, ColumnType.Numeric, 5)]
        public void Apply_Exp_Success(object[] values, ColumnType columnType, int index)
        {
            var i = 0;
            var objs = values.Select(x => new object[] { ++i, x }).ToArray();

            var args = new ObjectsResultSetResolverArgs(objs);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var alias = Mock.Of<IColumnAlias>(x => x.Column == 1 && x.Name == "myValue");
            var exp = Mock.Of<IColumnExpression>(x => x.Name=="exp" && x.Value == "myValue % 10");

            var ranking = new BottomRanking(new ColumnNameIdentifier("exp"), columnType, Enumerable.Repeat(alias, 1), Enumerable.Repeat(exp, 1));
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.RowCount, Is.EqualTo(1));
            Assert.That(filteredRs[0][0], Is.EqualTo(index));
            Assert.That(filteredRs[0][1], Is.EqualTo("125"));
        }

    }
}
