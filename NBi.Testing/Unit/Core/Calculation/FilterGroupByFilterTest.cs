using NBi.Core.Calculation;
using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsIndexResultSet;

namespace NBi.Testing.Unit.Core.Calculation
{
    public class FilterGroupByFilterTest
    {
        [Test]
        public void Execute_Top2OneKey_ResultSetReduced()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "alpha", 2 }, new object[] { "beta", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var settings = new SettingsIndexResultSet(KeysChoice.First, ValuesChoice.None, NumericAbsoluteTolerance.None);
            var grouping = new IndexByColumnGrouping(settings);

            var filter = new TopRanking(2, new ColumnPositionIdentifier(1), ColumnType.Numeric);

            var rankingByGroup = new FilterGroupByFilter(filter, grouping);

            var result = rankingByGroup.Apply(rs);
            Assert.That(result.Table.Rows, Has.Count.EqualTo(3));
            Assert.That(result.Table.Rows.Cast<DataRow>().Where(x => x[0].ToString()=="alpha").Count(), Is.EqualTo(2));
            Assert.That(result.Table.Rows.Cast<DataRow>().Where(x => x[0].ToString() == "beta").Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_Top2None_ResultSetReduced()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "alpha", 2 }, new object[] { "beta", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var filter = new TopRanking(2, new ColumnPositionIdentifier(1), ColumnType.Numeric);

            var rankingByGroup = new FilterGroupByFilter(filter, new NoneGrouping());

            var result = rankingByGroup.Apply(rs);
            Assert.That(result.Table.Rows, Has.Count.EqualTo(2));
            Assert.That(result.Table.Rows.Cast<DataRow>().Where(x => x[0].ToString() == "alpha").Count(), Is.EqualTo(1));
            Assert.That(result.Table.Rows.Cast<DataRow>().Where(x => x[0].ToString() == "beta").Count(), Is.EqualTo(1));
        }
    }
}
