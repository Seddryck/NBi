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
    public class NameColumnGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", 1], ["alpha", 2], ["beta", 3], new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)?.Rename("first");

            var settings = new SettingsNameResultSet(
                [
                    new Column(new ColumnNameIdentifier("first"), ColumnRole.Key, ColumnType.Text ),
                ]
            );
            var grouping = new NameColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(["alpha"])].Rows.Count, Is.EqualTo(3));
            Assert.That(result[new KeyCollection(["beta"])].Rows.Count , Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", 10 }, ["alpha", "1", 20], ["beta", "2", 30], ["alpha", "2", 40] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)?.Rename("first");
            rs.GetColumn(1)?.Rename("second");

            var settings = new SettingsNameResultSet(
                [
                    new Column(new ColumnNameIdentifier("first"), ColumnRole.Key, ColumnType.Text ),
                    new Column(new ColumnNameIdentifier("second"), ColumnRole.Key, ColumnType.Text ),
                ]
            );
            var grouping = new NameColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(["alpha", "1"])].Rows.Count, Is.EqualTo(2));
            Assert.That(result[new KeyCollection(["alpha", "2"])].Rows.Count, Is.EqualTo(1));
            Assert.That(result[new KeyCollection(["beta", "2"])].Rows.Count , Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoCustomColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", 1d, 10], ["alpha", 1, 20], new object[] { "beta", 2, 30 }, ["alpha", 2, 40] });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.GetColumn(0)?.Rename("first");
            rs.GetColumn(1)?.Rename("second");
            rs.GetColumn(1)?.Move(0);

            var settings = new SettingsNameResultSet(
                [
                    new Column(new ColumnNameIdentifier("first") , ColumnRole.Key, ColumnType.Text ),
                    new Column(new ColumnNameIdentifier("second"), ColumnRole.Key, ColumnType.Numeric ),
                ]
            );
            var grouping = new NameColumnGrouping(settings, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(["alpha", 1m])].Rows.Count, Is.EqualTo(2));
            Assert.That(result[new KeyCollection(["alpha", 2m])].Rows.Count, Is.EqualTo(1));
            Assert.That(result[new KeyCollection(["beta", 2m])].Rows.Count, Is.EqualTo(1));
        }    
    }
}
