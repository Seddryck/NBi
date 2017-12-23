using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Framework;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Framework.FailureMessage.Json
{
    public class DataRowsMessageJsonTest
    {
        #region Helpers
        private IEnumerable<DataRow> GetDataRows(int count)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < count; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            return dataTable.Rows.Cast<DataRow>();
        }
        #endregion

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectNumberOfRowsOnTop()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var samplers = new SamplersFactory<DataRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();

            Assert.That(value, Is.StringContaining("\"total-rows\":20"));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeaderAndSeparation()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var samplers = new SamplersFactory<DataRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            Assert.That(value, Is.StringContaining("\"sampled-rows\":10"));

            value = value.Substring(value.IndexOf("\"rows\""));
            Assert.That(value.Count(x => x == '['), Is.EqualTo(10 + 1));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 12;

            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var samplers = new SamplersFactory<DataRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(dataTable.Rows.Cast<DataRow>(), null, null);

            var value = msg.RenderExpected();
            Assert.That(value, Is.Not.StringContaining("\"sampled-rows\":"));

            value = value.Substring(value.IndexOf("\"rows\""));
            Assert.That(value.Count(x => x == '['), Is.EqualTo(rowCount + 1));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount + 20;
            var max = threshold / 2;

            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Sample
            );
            var samplers = new SamplersFactory<DataRow>().Instantiate(profile);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            Assert.That(value, Is.Not.StringContaining("\"sampled-rows\":"));

            value = value.Substring(value.IndexOf("\"rows\""));

            Assert.That(value.Count(x => x == '['), Is.EqualTo(rowCount + 1));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountAndMoreThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Sample
            );
            var samplers = new SamplersFactory<DataRow>().Instantiate(profile);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            Assert.That(value, Is.StringContaining($"\"total-rows\":{rowCount}"));
            Assert.That(value, Is.StringContaining($"\"sampled-rows\":{max}"));

            value = value.Substring(value.IndexOf("\"rows\""));

            Assert.That(value.Count(x => x == '['), Is.EqualTo(max + 1));
        }


        [Test]
        [TestCase(0, 5, 5, 5, 5, "missing")]
        [TestCase(5, 0, 5, 5, 5, "unexpected")]
        [TestCase(5, 5, 0, 5, 5, "duplicated")]
        [TestCase(5, 5, 5, 5, 0, "non-matching")]
        public void RenderCompared_NoSpecialRows_ReportMinimalInformation(
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

            var samplers = new SamplersFactory<DataRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(null, null, compared);
            var value = msg.RenderAnalysis();

            Assert.That(value, Is.StringContaining($"\"{expectedText}\":{{\"total-rows\":0}}"));
        }

        [Test]
        [TestCase(3, 0, 0, 0, 0, "missing")]
        [TestCase(0, 3, 0, 0, 0, "unexpected")]
        [TestCase(0, 0, 3, 0, 0, "duplicated")]
        [TestCase(0, 0, 0, 0, 3, "non-matching")]
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


            var samplers = new SamplersFactory<DataRow>().Instantiate(FailureReportProfile.Default);
            var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
            msg.BuildComparaison(null, null, compared);
            var value = msg.RenderAnalysis();

            Assert.That(value, Is.StringContaining($"\"{expectedText}\":{{\"total-rows\":3"));
            Assert.That(value, Is.Not.StringContaining($"\"{expectedText}\":{{\"total-rows\":3}}}}"));
        }
    }
}
