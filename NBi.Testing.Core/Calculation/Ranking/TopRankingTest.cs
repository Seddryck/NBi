using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Calculation.Ranking
{
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

            var ranking = new TopRanking(new ColumnOrdinalIdentifier(1), columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(1));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index.ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Max()));
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

            var ranking = new TopRanking(2, new ColumnOrdinalIdentifier(1), columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(2));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index[0].ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Max()));
            Assert.That(filteredRs.Rows[1].ItemArray[0], Is.EqualTo(index[1].ToString()));
            Assert.That(filteredRs.Rows[1].ItemArray[1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Max(), 1)).Max()));
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

            var ranking = new TopRanking(10, new ColumnOrdinalIdentifier(1), columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(values.Count()));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index[0].ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Max()));
            Assert.That(filteredRs.Rows[1].ItemArray[0], Is.EqualTo(index[1].ToString()));
            Assert.That(filteredRs.Rows[1].ItemArray[1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Max(), 1)).Max()));
            Assert.That(filteredRs.Rows[values.Count() - 1].ItemArray[0], Is.EqualTo(index[2].ToString()));
            Assert.That(filteredRs.Rows[values.Count() - 1].ItemArray[1], Is.EqualTo(values.Min()));
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

            var ranking = new TopRanking(new ColumnNameIdentifier("myValue"), columnType, Enumerable.Repeat(alias, 1), null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(1));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index.ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Max()));
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

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(1));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index.ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo("139"));
        }

    }
}
