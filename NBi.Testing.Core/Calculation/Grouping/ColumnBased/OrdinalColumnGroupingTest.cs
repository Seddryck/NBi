using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsOrdinalResultSet;

namespace NBi.Core.Testing.Calculation.Grouping.ColumnBased
{
    public class OrdinalColumnGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, ["alpha", 2], ["beta", 3], ["alpha", 4] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var settings = new SettingsOrdinalResultSet(KeysChoice.First, ValuesChoice.None, NumericAbsoluteTolerance.None);
            var grouping = new OrdinalColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(new[] { "alpha" })].Rows.Count, Is.EqualTo(3));
            Assert.That( result[new KeyCollection(new[] { "beta" })].Rows.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", 10 }, ["alpha", "1", 20], ["beta", "2", 30], ["alpha", "2", 40] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var settings = new SettingsOrdinalResultSet(KeysChoice.AllExpectLast, ValuesChoice.None, NumericAbsoluteTolerance.None);
            var grouping = new OrdinalColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(["alpha", "1"])].Rows.Count, Is.EqualTo(2));
            Assert.That(result[new KeyCollection(["alpha", "2"])].Rows.Count, Is.EqualTo(1));
            Assert.That(result[new KeyCollection(["beta", "2"])].Rows.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoCustomColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", 1d, 10], ["alpha", 1, 20], new object[] { "beta", 2, 30 }, ["alpha", 2, 40] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var settings = new SettingsOrdinalResultSet(
                [
                    new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Key, ColumnType.Text ),
                    new Column(new ColumnOrdinalIdentifier(1), ColumnRole.Key, ColumnType.Numeric ),
                ]
            );
            var grouping = new OrdinalColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(["alpha", 1m])].Rows.Count, Is.EqualTo(2));
            Assert.That(result[new KeyCollection(["alpha", 2m])].Rows.Count, Is.EqualTo(1));
            Assert.That(result[new KeyCollection(["beta", 2m])].Rows.Count, Is.EqualTo(1));
        }
    }
}
