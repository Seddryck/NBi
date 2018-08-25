using NBi.Core.Calculation.Grouping;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NBi.Core.ResultSet.SettingsIndexResultSet;

namespace NBi.Testing.Unit.Core.Calculation.Grouping
{
    public class NameByColumnGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "alpha", 2 }, new object[] { "beta", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "first";

            var settings = new SettingsNameResultSet(new List<IColumnDefinition>()
                {
                    new Column() { Name = "first", Role = ColumnRole.Key, Type = ColumnType.Text },
                }
            );
            var grouping = new NameByColumnGrouping(settings);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(new[] { "alpha" })].Rows, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(new[] { "beta" })].Rows, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", 10 }, new object[] { "alpha", "1", 20 }, new object[] { "beta", "2", 30 }, new object[] { "alpha", "2", 40 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "first";
            rs.Columns[1].ColumnName = "second";

            var settings = new SettingsNameResultSet(new List<IColumnDefinition>()
                {
                    new Column() { Name = "first", Role = ColumnRole.Key, Type = ColumnType.Text },
                    new Column() { Name = "second", Role = ColumnRole.Key, Type = ColumnType.Text },
                }
            );
            var grouping = new NameByColumnGrouping(settings);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(new object[] { "alpha", "1" })].Rows, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(new object[] { "alpha", "2" })].Rows, Has.Count.EqualTo(1));
            Assert.That(result[new KeyCollection(new object[] { "beta", "2" })].Rows, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_TwoCustomColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1d, 10 }, new object[] { "alpha", 1, 20 }, new object[] { "beta", 2, 30 }, new object[] { "alpha", 2, 40 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "first";
            rs.Columns[1].ColumnName = "second";
            rs.Columns[1].SetOrdinal(0);

            var settings = new SettingsNameResultSet(new List<IColumnDefinition>()
                {
                    new Column() { Name = "first", Role = ColumnRole.Key, Type = ColumnType.Text },
                    new Column() { Name = "second", Role = ColumnRole.Key, Type = ColumnType.Numeric },
                }
            );
            var grouping = new NameByColumnGrouping(settings);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(new object[] { "alpha", 1m })].Rows, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(new object[] { "alpha", 2m })].Rows, Has.Count.EqualTo(1));
            Assert.That(result[new KeyCollection(new object[] { "beta", 2m })].Rows, Has.Count.EqualTo(1));
        }
    }
}
