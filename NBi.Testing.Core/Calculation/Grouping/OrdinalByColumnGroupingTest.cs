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
using static NBi.Core.ResultSet.SettingsOrdinalResultSet;

namespace NBi.Testing.Core.Calculation.Grouping
{
    public class OrdinalByColumnGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "alpha", 2 }, new object[] { "beta", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var settings = new SettingsOrdinalResultSet(KeysChoice.First, ValuesChoice.None, NumericAbsoluteTolerance.None);
            var grouping = new OrdinalByColumnGrouping(settings);

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

            var settings = new SettingsOrdinalResultSet(KeysChoice.AllExpectLast, ValuesChoice.None, NumericAbsoluteTolerance.None);
            var grouping = new OrdinalByColumnGrouping(settings);

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

            var settings = new SettingsOrdinalResultSet(new List<IColumnDefinition>()
                {
                    new Column() { Identifier = new ColumnOrdinalIdentifier(0), Role = ColumnRole.Key, Type = ColumnType.Text },
                    new Column() { Identifier = new ColumnOrdinalIdentifier(1), Role = ColumnRole.Key, Type = ColumnType.Numeric },
                }
            );
            var grouping = new OrdinalByColumnGrouping(settings);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[new KeyCollection(new object[] { "alpha", 1m })].Rows, Has.Count.EqualTo(2));
            Assert.That(result[new KeyCollection(new object[] { "alpha", 2m })].Rows, Has.Count.EqualTo(1));
            Assert.That(result[new KeyCollection(new object[] { "beta", 2m })].Rows, Has.Count.EqualTo(1));
        }
    }
}
