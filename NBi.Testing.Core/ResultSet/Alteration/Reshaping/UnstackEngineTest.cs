using NBi.Core.ResultSet.Alteration.Reshaping;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Data;
using System.Diagnostics;

namespace NBi.Testing.Core.ResultSet.Alteration.Reshaping
{
    public class UnstackEngineTest
    {
        [Test]
        public void Execute_SingleKeySingleValue_ExpectedResultSet()
        {
            var resolver = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { "alpha", "A", 1 },
                        new object[] { "alpha", "B", 2 },
                        new object[] { "beta", "A", 3 },
                        new object[] { "beta", "B", 4 }
                    }
                ));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "headerColumn";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var unstack = new UnstackEngine(args);
            var result = unstack.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(3));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "keyColumn"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B"));
            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["A"]) == 1);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["B"]) == 2);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["A"]) == 3);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["B"]) == 4);
        }

        [Test]
        public void Execute_SingleKeyMultipleValue_ExpectedResultSet()
        {
            var resolver = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { "alpha", "A", 1, -1 },
                        new object[] { "alpha", "B", 2, -2 },
                        new object[] { "beta", "A", 3, -3 },
                        new object[] { "beta", "B", 4, -4 }
                    }
                ));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "headerColumn";
            rs.Columns[2].ColumnName = "value1Column";
            rs.Columns[3].ColumnName = "value2Column";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var unstack = new UnstackEngine(args);
            var result = unstack.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(5));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "keyColumn"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A_value1Column"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B_value1Column"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A_value2Column"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B_value2Column"));
            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["A_value1Column"]) == 1);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["B_value1Column"]) == 2);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["A_value1Column"]) == 3);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["B_value1Column"]) == 4);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["A_value2Column"]) == -1);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["B_value2Column"]) == -2);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["A_value2Column"]) == -3);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["B_value2Column"]) == -4);
        }

        [Test]
        public void Execute_MultipleKeysSingleValue_ExpectedResultSet()
        {
            var resolver = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { "alpha", "one", "A", 1 },
                        new object[] { "alpha", "one", "B", 2 },
                        new object[] { "beta", "one", "A", 3 },
                        new object[] { "beta", "one", "B", 4 },
                        new object[] { "beta", "two", "B", -4 }
                    }
                ));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "key1Column";
            rs.Columns[1].ColumnName = "key2Column";
            rs.Columns[2].ColumnName = "headerColumn";
            rs.Columns[3].ColumnName = "valueColumn";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    {
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("key1Column") && x.Type == ColumnType.Text),
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("key2Column") && x.Type == ColumnType.Text),
                    }
                );

            var unstack = new UnstackEngine(args);
            var result = unstack.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(4));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "key1Column"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "key2Column"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B"));
            Assert.That(result.Rows.Count, Is.EqualTo(3));
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "alpha" && x["key2Column"] as string == "one")["A"]) == 1);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "alpha" && x["key2Column"] as string == "one")["B"]) == 2);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "one")["A"]) == 3);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "one")["B"]) == 4);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "two")["B"]) == -4);
            Assert.That(result.Rows.Cast<DataRow>().Single(x => x["key1Column"] as string == "beta" && x["key2Column"] as string == "two")["A"] == DBNull.Value);
        }

        [Test]
        public void Execute_EnforcedColumnsThatWasNotExpected_ExpectedResultSet()
        {
            var resolver = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { "alpha", "A", 1 },
                        new object[] { "alpha", "B", 2 },
                        new object[] { "beta", "A", 3 },
                        new object[] { "beta", "B", 4 }
                    }
                ));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "headerColumn";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) },
                    new List<ColumnNameIdentifier>() {  new ColumnNameIdentifier("C") }
                );

            var unstack = new UnstackEngine(args);
            var result = unstack.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(4));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "keyColumn"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "C"));
            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["C"] == DBNull.Value);
            Assert.That(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["C"] == DBNull.Value);
        }

        [Test]
        public void Execute_EnforcedColumnsThatWasAlreadyExpected_ExpectedResultSet()
        {
            var resolver = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { "alpha", "A", 1 },
                        new object[] { "alpha", "B", 2 },
                        new object[] { "beta", "A", 3 },
                        new object[] { "beta", "B", 4 }
                    }
                ));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "headerColumn";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) },
                    new List<ColumnNameIdentifier>() { new ColumnNameIdentifier("A"), new ColumnNameIdentifier("B") }
                );

            var unstack = new UnstackEngine(args);
            var result = unstack.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(3));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "keyColumn"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "A"));
            Assert.That(result.Columns.Cast<DataColumn>().Any(x => x.ColumnName == "B"));
            Assert.That(result.Columns.Cast<DataColumn>().Count(x => x.ColumnName == "A") == 1);
            Assert.That(result.Columns.Cast<DataColumn>().Count(x => x.ColumnName == "B") == 1);
        }

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        //[TestCase(40000)]
        [Retry(3)]
        [Parallelizable(ParallelScope.Self)]
        public void Execute_LargeResultSet_ExpectedPerformance(int count)
        {
            var values = new List<object>();
            for (int i = 0; i < count; i++)
                values.Add(new object[] { i % 2 == 0 ? "alpha" : "beta", (i/2) % (count/2), 1 });

            var resolver = new ObjectsResultSetResolver(new ObjectsResultSetResolverArgs(values));
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "headerColumn";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new UnstackArgs(
                    new ColumnNameIdentifier("headerColumn"),
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var unstack = new UnstackEngine(args);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = unstack.Execute(rs);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(15));
        }
    }
}
