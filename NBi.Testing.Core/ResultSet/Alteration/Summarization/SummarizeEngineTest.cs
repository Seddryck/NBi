using NBi.Core.ResultSet.Alteration.Summarization;
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

namespace NBi.Testing.Core.ResultSet.Alteration.Summarization
{
    public class SummarizeEngineTest
    {
        private NBi.Core.ResultSet.ResultSet Build()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "alpha", 2 }, new object[] { "beta", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "valueColumn";
            return rs;
        }

        private NBi.Core.ResultSet.ResultSet BuildLarge(int count)
        {
            var values = new List<object>();
            for (int i = 0; i < count; i++)
                values.Add(new object[] { i % 2 == 0 ? "alpha" : "beta", 1 });

            var args = new ObjectsResultSetResolverArgs(values);
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "keyColumn";
            rs.Columns[1].ColumnName = "valueColumn";
            return rs;
        }

        [Test]
        public void Execute_SingleKeySingleAggregation_ExpectedResultSet()
        {
            var rs = Build();

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    { new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric) },
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(2));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["keyColumn"] as string == "alpha"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["keyColumn"] as string == "beta"));
            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["valueColumn"]) == 7);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta")["valueColumn"]) == 3);
        }

        [Test]
        public void Execute_MultipleKeySingleAggregation_ExpectedResultSet()
        {
            var rs = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] { new object[] { "alpha", "foo", 1 }, new object[] { "alpha", "foo", 2 }, new object[] { "beta", "foo", 3 }, new object[] { "alpha", "bar", 4 } })
                ).Execute();
            rs.Columns[0].ColumnName = "ColumnA";
            rs.Columns[1].ColumnName = "ColumnB";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    { new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric) },
                    new List<IColumnDefinitionLight>()
                    {
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("ColumnA") && x.Type == ColumnType.Text),
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("ColumnB") && x.Type == ColumnType.Text)
                    }
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(3));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnA"] as string == "alpha" && x["ColumnB"] as string == "foo"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnA"] as string == "beta"  && x["ColumnB"] as string == "foo"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnA"] as string == "alpha" && x["ColumnB"] as string == "bar"));
            Assert.That(result.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void Execute_MultipleKeyNonAlphabeticalOrderSingleAggregation_ExpectedResultSet()
        {
            var rs = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] { new object[] { "alpha", "foo", 1 }, new object[] { "alpha", "foo", 2 }, new object[] { "beta", "foo", 3 }, new object[] { "alpha", "bar", 4 } })
                ).Execute();
            rs.Columns[0].ColumnName = "ColumnB";
            rs.Columns[1].ColumnName = "ColumnA";
            rs.Columns[2].ColumnName = "valueColumn";

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    { new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric) },
                    new List<IColumnDefinitionLight>()
                    {
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("ColumnB") && x.Type == ColumnType.Text),
                        Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("ColumnA") && x.Type == ColumnType.Text),
                    }
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(3));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnB"] as string == "alpha" && x["ColumnA"] as string == "foo"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnB"] as string == "beta"  && x["ColumnA"] as string == "foo"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["ColumnB"] as string == "alpha" && x["ColumnA"] as string == "bar"));
            Assert.That(result.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void Execute_SingleKeyMultipleAggregations_ExpectedResultSet()
        {
            var rs = Build();

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Min, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Max, ColumnType.Numeric),
                    },
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["keyColumn"] as string == "alpha"));
            Assert.That(result.Rows.Cast<DataRow>().Any(x => x["keyColumn"] as string == "beta"));
            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["valueColumn_Sum"]) == 7);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["valueColumn_Min"]) == 1);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "alpha")["valueColumn_Max"]) == 4);
            Assert.That(Convert.ToInt32(result.Rows.Cast<DataRow>().Single(x => x["keyColumn"] as string == "beta").ItemArray.Skip(1).Distinct().Single()) == 3);
        }


        [Test]
        public void Execute_NoKeySingleAggregations_ExpectedResultSet()
        {
            var rs = Build();

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric),
                    },
                    new List<IColumnDefinitionLight>()
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(1));
            Assert.That(result.Rows.Count, Is.EqualTo(1));
            Assert.That(Convert.ToInt32(result.Rows[0][0]) == 10);
        }

        [Test]
        public void Execute_NoKeyMultipleAggregations_ExpectedResultSet()
        {
            var rs = Build();

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Min, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Max, ColumnType.Numeric),
                    },
                    new List<IColumnDefinitionLight>()
                );

            var summarize = new SummarizeEngine(args);
            var result = summarize.Execute(rs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Columns.Count, Is.EqualTo(3));
            Assert.That(result.Rows.Count, Is.EqualTo(1));
            Assert.That(Convert.ToInt32(result.Rows[0][0]) == 10);
            Assert.That(Convert.ToInt32(result.Rows[0][1]) == 1);
            Assert.That(Convert.ToInt32(result.Rows[0][2]) == 4);
        }

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [Retry(3)]
        public void Execute_LargeResultSet_ExpectedPerformance(int count)
        {
            var rs = BuildLarge(count);

            var args = new SummarizeArgs(
                    new List<ColumnAggregationArgs>()
                    {
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Sum, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Min, ColumnType.Numeric),
                        new ColumnAggregationArgs(new ColumnNameIdentifier("valueColumn"), AggregationFunctionType.Max, ColumnType.Numeric),
                    },
                    new List<IColumnDefinitionLight>()
                    { Mock.Of<IColumnDefinitionLight>(x => x.Identifier == new ColumnNameIdentifier("keyColumn") && x.Type == ColumnType.Text) }
                );

            var summarize = new SummarizeEngine(args);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = summarize.Execute(rs);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(10));
        }

    }
}
