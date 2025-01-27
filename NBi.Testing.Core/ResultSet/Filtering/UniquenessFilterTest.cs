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

public class UniquenessFilterTest
{
    [Test]
    public void Execute_OnlyUniqueRows_ResultSetConstant()
    {
        var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, ["alpha", 2], ["beta", 3], ["alpha", 4] });
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var settings = new SettingsOrdinalResultSet(KeysChoice.All, ValuesChoice.None, NumericAbsoluteTolerance.None);
        var grouping = new OrdinalColumnGrouping(settings, Context.None);

        var uniquenessFilter = new UniquenessFilter(grouping);

        var result = uniquenessFilter.Apply(rs);
        Assert.That(result.Columns.Count(), Is.EqualTo(2));
        Assert.That(result.Rows.Count(), Is.EqualTo(4));
    }

    [Test]
    public void Execute_SomeDuplicatesButNotOnValues_ResultSetReduced()
    {
        var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, ["alpha", 2], ["beta", 3], ["alpha", 4] });
        var resolver = new ObjectsResultSetResolver(args);
        var rs = resolver.Execute();

        var settings = new SettingsOrdinalResultSet(KeysChoice.First, ValuesChoice.None, NumericAbsoluteTolerance.None);
        var grouping = new OrdinalColumnGrouping(settings, Context.None);

        var uniquenessFilter = new UniquenessFilter(grouping);

        var result = uniquenessFilter.Apply(rs);
        Assert.That(result.Columns.Count(), Is.EqualTo(2));
        Assert.That(result.Rows.Count(), Is.EqualTo(1));
    }
}
