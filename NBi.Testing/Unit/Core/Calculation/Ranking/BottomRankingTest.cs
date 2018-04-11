using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Calculation.Ranking
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

            var ranking = new BottomRanking("#1", columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(1));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index.ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Min()));
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

            var ranking = new BottomRanking(2, "#1", columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(2));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index[0].ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Min()));
            Assert.That(filteredRs.Rows[1].ItemArray[0], Is.EqualTo(index[1].ToString()));
            Assert.That(filteredRs.Rows[1].ItemArray[1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Min(), 1)).Min()));
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

            var ranking = new BottomRanking(10, "#1", columnType, null, null);
            var filteredRs = ranking.Apply(rs);

            Assert.That(filteredRs.Rows.Count, Is.EqualTo(values.Count()));
            Assert.That(filteredRs.Rows[0].ItemArray[0], Is.EqualTo(index[0].ToString()));
            Assert.That(filteredRs.Rows[0].ItemArray[1], Is.EqualTo(values.Min()));
            Assert.That(filteredRs.Rows[1].ItemArray[0], Is.EqualTo(index[1].ToString()));
            Assert.That(filteredRs.Rows[1].ItemArray[1], Is.EqualTo(values.Except(Enumerable.Repeat(values.Min(), 1)).Min()));
            Assert.That(filteredRs.Rows[values.Count() - 1].ItemArray[0], Is.EqualTo(index[2].ToString()));
            Assert.That(filteredRs.Rows[values.Count() - 1].ItemArray[1], Is.EqualTo(values.Max()));
        }

    }
}
