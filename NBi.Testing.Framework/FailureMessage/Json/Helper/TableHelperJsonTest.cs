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
using NBi.Extensibility;

namespace NBi.Framework.Testing.FailureMessage.Json.Helper
{
    public class TableHelperJsonTest
    {
        [Test]
        public void Build_ThreeColumnsTwoRowsNotSampled_RowCountsSpecified()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(["Alpha", 10, true], false);
            dataTable.LoadDataRow(["Beta", 20, false], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            using var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new FullSampler<IResultRow>(), writer);
            Console.WriteLine(sb.ToString());
            Assert.That(sb.ToString, Does.Contain("total-rows"));
            Assert.That(sb.ToString, Does.Not.Contain("sampled-rows"));
        }
        [Test]
        public void Build_ThreeColumnsTwoRowsSampled_RowCountsSpecified()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(["Alpha", 10, true], false);
            dataTable.LoadDataRow(["Beta", 20, false], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            using var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new BasicSampler<IResultRow>(1, 1), writer);
            Assert.That(sb.ToString, Does.Contain("total-rows"));
            Assert.That(sb.ToString, Does.Contain("sampled-rows"));
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ColumnsSpecified()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(["Alpha", 10, true], false);
            dataTable.LoadDataRow(["Beta", 20, false], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new FullSampler<IResultRow>(), writer);

            Assert.That(sb.ToString, Does.Contain("columns"));
            Assert.That(sb.ToString, Does.Contain("Id").And.Contain("Numeric value").And.Contain("Boolean value"));
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ColumnsPropertiesSpecified()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Boolean value"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Boolean value"]!.ExtendedProperties["NBi::Type"] = ColumnType.Boolean;
            dataTable.LoadDataRow(["Alpha", 10, true], false);
            dataTable.LoadDataRow(["Beta", 20, false], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new FullSampler<IResultRow>(), writer);

            Assert.That(sb.ToString, Does.Contain("\"role\":\"KEY\""));
            Assert.That(sb.ToString, Does.Contain("\"role\":\"VALUE\""));
            Assert.That(sb.ToString, Does.Contain("\"type\":\"Numeric\""));
            Assert.That(sb.ToString, Does.Contain("\"type\":\"Boolean\""));
            Assert.That(sb.ToString, Does.Not.Contain("\"tolerance\""));
        }

        [Test]
        public void Build_ThreeColumnsTwoRows_ToleranceSpecified()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Value;
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Tolerance"] = new NumericAbsoluteTolerance(10, SideTolerance.Both);
            dataTable.LoadDataRow(["Alpha", 10], false);
            dataTable.LoadDataRow(["Beta", 20], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new FullSampler<IResultRow>(), writer);

            Assert.That(sb.ToString, Does.Contain("\"tolerance\":\"(+/- 10)\""));
        }

        [Test]
        [TestCase("fr-fr")]
        [TestCase("en-us")]
        [TestCase("de-de")]
        public void Build_ThreeColumnsTwoRows_RowsSpecified(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add("Numeric value", typeof(decimal));
            dataTable.Columns["Numeric value"]!.ExtendedProperties["NBi::Type"] = ColumnType.Numeric;
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Boolean value"]!.ExtendedProperties["NBi::Type"] = ColumnType.Boolean;
            dataTable.Columns.Add("DateTime value", typeof(DateTime));
            dataTable.Columns["DateTime value"]!.ExtendedProperties["NBi::Type"] = ColumnType.DateTime;
            dataTable.LoadDataRow(["Alpha", 10.5, true, new DateTime(2010, 5, 6)], false);
            dataTable.LoadDataRow(["Beta", 20, false, new DateTime(2010, 5, 15, 7, 45, 12)], false);
            var rs = new DataTableResultSet(dataTable);

            var helper = new TableHelperJson();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using var writer = new JsonTextWriter(sw);
            helper.Execute(rs.Rows, new FullSampler<IResultRow>(), writer);

            Assert.That(sb.ToString, Does.Contain("rows"));
            Assert.That(sb.ToString, Does.Contain("[\"Alpha\",\"10.5\",\"True\",\"2010-05-06\"]"));
            Assert.That(sb.ToString, Does.Contain("[\"Beta\",\"20\",\"False\",\"2010-05-15 07:45:12\"]"));
        }
    }
}
