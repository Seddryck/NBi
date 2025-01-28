using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Extensibility;
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

namespace NBi.Framework.Testing.FailureMessage.Json;

public class DataRowsMessageJsonTest
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
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison(rs.Rows, [], null);
        var value = msg.RenderExpected();

        Assert.That(value, Does.Contain("\"total-rows\":20"));
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
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison(rs.Rows, [], null);
        var value = msg.RenderExpected();
        Assert.That(value, Does.Contain("\"sampled-rows\":10"));

        value = value[value.IndexOf("\"rows\"")..];
        Assert.That(value.Count(x => x == '['), Is.EqualTo(10 + 1));
    }

    [Test]
    public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
    {
        var rowCount = 12;

        var dataTable = new DataTable() { TableName = "MyTable" };
        dataTable.Columns.Add(new DataColumn("Id"));
        dataTable.Columns.Add(new DataColumn("Numeric value"));
        dataTable.Columns.Add(new DataColumn("Boolean value"));
        for (int i = 0; i < rowCount; i++)
            dataTable.LoadDataRow(["Alpha", i, true], false);
        var rs = new DataTableResultSet(dataTable);

        var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison(rs.Rows, [], null);

        var value = msg.RenderExpected();
        Assert.That(value, Does.Not.Contain("\"sampled-rows\":"));

        value = value[value.IndexOf("\"rows\"")..];
        Assert.That(value.Count(x => x == '['), Is.EqualTo(rowCount + 1));
    }

    [Test]
    public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
    {
        var rowCount = 120;
        var threshold = rowCount + 20;
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
            && p.ExpectedSet == FailureReportSetType.Sample
        );
        var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison(rs.Rows, [], null);
        var value = msg.RenderExpected();
        Assert.That(value, Does.Not.Contain("\"sampled-rows\":"));

        value = value[value.IndexOf("\"rows\"")..];

        Assert.That(value.Count(x => x == '['), Is.EqualTo(rowCount + 1));
    }

    [Test]
    public void RenderExpected_MoreThanSampleRowsCountAndMoreThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
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
            && p.ExpectedSet == FailureReportSetType.Sample
        );
        var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison(rs.Rows, [], null);
        var value = msg.RenderExpected();
        Assert.That(value, Does.Contain($"\"total-rows\":{rowCount}"));
        Assert.That(value, Does.Contain($"\"sampled-rows\":{max}"));

        value = value[value.IndexOf("\"rows\"")..];

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

        var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison([], [], compared);
        var value = msg.RenderAnalysis();

        Assert.That(value, Does.Contain($"\"{expectedText}\":{{\"total-rows\":0}}"));
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


        var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        msg.BuildComparaison([], [], compared);
        var value = msg.RenderAnalysis();

        Assert.That(value, Does.Contain($"\"{expectedText}\":{{\"total-rows\":3"));
        Assert.That(value, Does.Not.Contain($"\"{expectedText}\":{{\"total-rows\":3}}}}"));
    }

    [Test]
    public void RenderMessage_NoAdditional_IncludeTimestamp()
    {
        var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
        var msg = new DataRowsMessageJson(EngineStyle.ByIndex, samplers);
        var value = msg.RenderMessage();

        Assert.That(value, Does.Contain($"\"timestamp\":\"{DateTime.Now.Year}-"));
    }
}
