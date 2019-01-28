using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System.IO;
using NBi.Core.Scalar.Comparer;
using System.Globalization;
using System.Threading;
using NBi.Framework.FailureMessage.Json.Helper;

namespace NBi.Testing.Unit.Framework.FailureMessage.Json.Helper
{
    public class TableHelperJsonTest
    {
        [Test]
        public void Build_ThreeColumnsTwoRowsNotSampled_RowCountsSpecified()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new FullSampler<DataRow>(), writer);
                Console.WriteLine(sb.ToString());
                Assert.That(sb.ToString, Is.StringContaining("total-rows"));
                Assert.That(sb.ToString, Is.Not.StringContaining("sampled-rows"));
            }
        }
        [Test]
        public void Build_ThreeColumnsTwoRowsSampled_RowCountsSpecified()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new BasicSampler<DataRow>(1, 1), writer);
                Console.WriteLine(sb.ToString());
                Assert.That(sb.ToString, Is.StringContaining("total-rows"));
                Assert.That(sb.ToString, Is.StringContaining("sampled-rows"));
            }
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ColumnsSpecified()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new FullSampler<DataRow>(), writer);

                Assert.That(sb.ToString, Is.StringContaining("columns"));
                Assert.That(sb.ToString, Is.StringContaining("Id").And.StringContaining("Numeric value").And.StringContaining("Boolean value"));
            }
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ColumnsPropertiesSpecified()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Boolean value"].ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Boolean value"].ExtendedProperties["NBi::Type"] = ColumnType.Boolean;
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new FullSampler<DataRow>(), writer);

                Assert.That(sb.ToString, Is.StringContaining("\"role\":\"KEY\""));
                Assert.That(sb.ToString, Is.StringContaining("\"role\":\"VALUE\""));
                Assert.That(sb.ToString, Is.StringContaining("\"type\":\"Numeric\""));
                Assert.That(sb.ToString, Is.StringContaining("\"type\":\"Boolean\""));
                Assert.That(sb.ToString, Is.Not.StringContaining("\"tolerance\""));
            }
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ToleranceSpecified()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Tolerance"] = new NumericAbsoluteTolerance(10, SideTolerance.Both);
            dataTable.LoadDataRow(new object[] { "Alpha", 10 }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20 }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new FullSampler<DataRow>(), writer);

                Assert.That(sb.ToString, Is.StringContaining("\"tolerance\":\"(+/- 10)\""));
            }
        }

        [Test]
        [TestCase("fr-fr")]
        [TestCase("en-us")]
        [TestCase("de-de")]
        public void Build_ThreeColumnsTwoRows_RowsSpecified(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add("Numeric value", typeof(decimal));
            dataTable.Columns["Numeric value"].ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Boolean value"].ExtendedProperties["NBi::Type"] = ColumnType.Boolean;
            dataTable.Columns.Add("DateTime value", typeof(DateTime));
            dataTable.Columns["DateTime value"].ExtendedProperties["NBi::Type"] = ColumnType.DateTime;
            dataTable.LoadDataRow(new object[] { "Alpha", 10.5, true, new DateTime(2010, 5, 6) }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false, new DateTime(2010, 5, 15, 7, 45, 12) }, false);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
            {
                helper.Execute(dataTable.Rows.Cast<DataRow>(), new FullSampler<DataRow>(), writer);

                Assert.That(sb.ToString, Is.StringContaining("rows"));
                Assert.That(sb.ToString, Is.StringContaining("[\"Alpha\",\"10.5\",\"True\",\"2010-05-06\"]"));
                Assert.That(sb.ToString, Is.StringContaining("[\"Beta\",\"20\",\"False\",\"2010-05-15 07:45:12\"]"));
            }
        }
    }
}
