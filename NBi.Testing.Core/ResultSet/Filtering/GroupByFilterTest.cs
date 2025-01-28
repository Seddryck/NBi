using NBi.Core.Calculation;
using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsOrdinalResultSet;

namespace NBi.Core.Testing.ResultSet.Filtering;

public class GroupByFilterTest
{
    [Test]
    public void Execute_Top2OneKey_ResultSetReduced()
    {
        var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, ["alpha", 2], ["beta", 3], ["alpha", 4] });
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var settings = new SettingsOrdinalResultSet(KeysChoice.First, ValuesChoice.None, NumericAbsoluteTolerance.None);
        var grouping = new OrdinalColumnGrouping(settings, Context.None);

        var filter = new TopRanking(2, new ColumnOrdinalIdentifier(1), ColumnType.Numeric);

        var rankingByGroup = new GroupByFilter(filter, grouping);

        var result = rankingByGroup.Apply(rs);
        Assert.That(result.Rows.Count(), Is.EqualTo(3));
        Assert.That(result.Rows.Where(x => x[0]?.ToString()=="alpha").Count(), Is.EqualTo(2));
        Assert.That(result.Rows.Where(x => x[0]?.ToString() == "beta").Count(), Is.EqualTo(1));
    }

    [Test]
    public void Execute_Top2None_ResultSetReduced()
    {
        var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, ["alpha", 2], ["beta", 3], ["alpha", 4] });
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var filter = new TopRanking(2, new ColumnOrdinalIdentifier(1), ColumnType.Numeric);

        var rankingByGroup = new GroupByFilter(filter, new NoneGrouping());

        var result = rankingByGroup.Apply(rs);
        Assert.That(result.Rows.Count(), Is.EqualTo(2));
        Assert.That(result.Rows.Where(x => x[0]?.ToString() == "alpha").Count(), Is.EqualTo(1));
        Assert.That(result.Rows.Where(x => x[0]?.ToString() == "beta").Count(), Is.EqualTo(1));
    }
}
