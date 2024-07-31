using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Framework;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Testing.FailureMessage.Markdown
{
    public class DataRowsMessageMardownTest
    {
        #region Helpers
        private static IEnumerable<IResultRow> GetDataRows(int count)
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < count; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            return rs.Rows;
        }
        #endregion

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectNumberOfRowsOnTop()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(lines[0], Is.EqualTo("Result-set with 20 rows"));
        }

        [Test]
        public void RenderExpected_OneRow_ReturnCorrectNumberOfRowsOnTopWithoutPlurial()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 1; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines[0], Is.EqualTo("Result-set with 1 row"));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeadersAndSeparation()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"]!.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(10 + 3));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeaderAndSeparation()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(10 + 3 -1)); //-1 because we've no ExtendedProperties
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 12;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"]!.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            
            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(rowCount + 3));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"]!.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Sample
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            
            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(max + 3));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfileFull_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"]!.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Full
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            
            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(rowCount + 3));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfileNone_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.None
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains('|')), Is.EqualTo(0));
            Assert.That(lines, Has.All.EqualTo("Display skipped."));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectCountOfSkippedRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 22; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            //Not exactly the last line but the previous due to paragraph rendering.
            var lastLine = lines.Reverse().ElementAt(1);

            Assert.That(lastLine, Is.EqualTo("12 (of 22) rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(5)]
        [TestCase(12)]
        public void RenderExpected_LessThanMaxRowsCount_DoesntDisplaySkippedRow(int rowCount)
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(["Alpha", i, true], false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(rs.Rows, [], null);
            var value = msg.RenderExpected();

            Assert.That(value, Does.Not.Contain("rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(0, 5, 5, 5, 5, "Missing rows:")]
        [TestCase(5, 0, 5, 5, 5, "Unexpected rows:")]
        [TestCase(5, 5, 0, 5, 5, "Duplicated rows:")]
        [TestCase(5, 5, 5, 5, 0, "Non matching value rows:")]
        public void RenderCompared_NoSpecialRows_DoesntDisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string unexpectedText)
        {
            var compared = ResultResultSet.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows( unexpectedRowCount)
                    , GetDataRows( duplicatedRowCount)
                    , GetDataRows( keyMatchingRowCount)
                    , GetDataRows( nonMatchingValueRowCount)
                );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison([], [], compared);
            var value = msg.RenderAnalysis();

            Assert.That(value, Does.Not.Contain(unexpectedText));
        }

        [Test]
        [TestCase(3, 0, 0, 0, 0, "Missing rows:")]
        [TestCase(0, 3, 0, 0, 0, "Unexpected rows:")]
        [TestCase(0, 0, 3, 0, 0, "Duplicated rows:")]
        [TestCase(0, 0, 0, 0, 3, "Non matching value rows:")]
        public void RenderCompared_WithSpecialRows_DisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string expectedText)
        {
            var compared = ResultResultSet.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                    , GetDataRows(duplicatedRowCount)
                    , GetDataRows(keyMatchingRowCount)
                    , GetDataRows(nonMatchingValueRowCount)
                );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageMarkdown(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison([], [], compared);
            var value = msg.RenderAnalysis();

            Assert.That(value, Does.Contain(expectedText));
        }
    }
}
