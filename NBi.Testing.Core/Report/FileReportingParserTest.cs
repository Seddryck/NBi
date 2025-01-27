using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core.Report;
using NUnit.Framework;
using System.Data;
using NBi.Testing;

namespace NBi.Core.Testing.Report;

[TestFixture]
public class FileReportingParserTest
{

    private string ReportFileDirectory { get; set; } = string.Empty;

    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {
        CreateReportFile("Currency_List");
        CreateReportFile("Currency_List - SProc");
        CreateReportFile("Currency_Rates");
        CreateReportFile("Employee_Sales_Summary");
        CreateSharedDataSet("EmployeeSalesDetail");
        CreateSharedDataSet("EmployeeSalesYearOverYear");
        CreateSharedDataSet("EmpSalesMonth");
        CreateSharedDataSet("SalesEmployees");
    }

    //Called only at instance destruction
    [OneTimeTearDown]
    public void TearDownMethods()
    {
    }

    //Called before each test
    [SetUp]
    public void SetupTest()
    {
        
    }

    //Called after each test
    [TearDown]
    public void TearDownTest()
    {
    }

    #endregion

    protected void CreateReportFile(string filename)
    {
        string file = @"\Temp\" + filename + ".rdl";
        var resource = "NBi.Core.Testing.Report.Resources." + filename + ".rdl";
        var physicalFilename = FileOnDisk.CreatePhysicalFile(file, resource);
        ReportFileDirectory = Path.GetDirectoryName(physicalFilename) + Path.DirectorySeparatorChar.ToString();
    }

    protected void CreateSharedDataSet(string filename)
    {
        string file = @"\Temp\" + filename + ".rsd";
        var resource = "NBi.Core.Testing.Report.Resources." + filename + ".rsd";
        var physicalFilename = FileOnDisk.CreatePhysicalFile(file, resource);
        ReportFileDirectory = Path.GetDirectoryName(physicalFilename) + Path.DirectorySeparatorChar.ToString();
    }

    [Test]
    public void ExtractQuery_ExistingReportAndDataSet_CorrectQueryReturned()
    {
        var request = new ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Currency_List"
                , "Currency"
            );

        var parser = new FileReportingParser();
        var query = parser.ExtractCommand(request);

        Assert.That(query.Text,
            Does.Contain("SELECT").And
            .Contain("[CurrencyAlternateKey]").And
            .Contain("[DimCurrency]"));
        Assert.That(query.CommandType, Is.EqualTo(CommandType.Text));
    }

    [Test]
    public void ExtractQuery_NonExistingDataSetOneExisting_CorrectExceptionReturned()
    {
        var request = new ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Currency_List"
                , "Non Existing"
            );

        var parser = new FileReportingParser();
        var ex = Assert.Throws<ArgumentException>(() => parser.ExtractCommand(request));
        Assert.That(ex?.Message, Does.Contain("'Currency'"));
    }

    [Test]
    public void ExtractQuery_NonExistingDataSetMoreThanOneExisting_CorrectExceptionReturned()
    {
        var request = new NBi.Core.Report.ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Currency_Rates"
                , "Non Existing"
            );

        var parser = new FileReportingParser();
        var ex = Assert.Throws<ArgumentException>(() => parser.ExtractCommand(request));
        Assert.That(ex?.Message, Does.Contain("DataSet1").And.Contain("DataSet2"));
    }

    [Test]
    public void ExtractQuery_NonExistingReport_CorrectExceptionReturned()
    {
        var request = new NBi.Core.Report.ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Not Existing"
                , "DataSet1"
            );

        var parser = new FileReportingParser();
        var ex = Assert.Throws<ArgumentException>(() => parser.ExtractCommand(request));
        Assert.That(ex?.Message, Does.Contain("No report found"));
    }

    [Test]
    public void ExtractQuery_ExistingReportAndSharedDataSet_CorrectQueryReturned()
    {
        var request = new NBi.Core.Report.ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Employee_Sales_Summary"
                , "SalesEmployees2008R2"
            );

        var parser = new FileReportingParser();
        var query = parser.ExtractCommand(request);

        Assert.That(query.Text,
            Does.Contain("SELECT").And
            .Contain("[Sales].[SalesPerson]").And
            .Contain("[HumanResources].[Employee]"));
        Assert.That(query.CommandType, Is.EqualTo(CommandType.Text));
    }

    [Test]
    public void ExtractQuery_ExistingSharedDataSet_CorrectQueryReturned()
    {
        var request = new SharedDatasetRequest(
                string.Empty
                , ReportFileDirectory
                , "SalesEmployees"
            );

        var parser = new FileReportingParser();
        var query = parser.ExtractCommand(request);

        Assert.That(query.Text,
            Does.Contain("SELECT").And
            .Contain("[Sales].[SalesPerson]").And
            .Contain("[HumanResources].[Employee]"));
        Assert.That(query.CommandType, Is.EqualTo(CommandType.Text));
    }

    [Test]
    public void ExtractSProc_ExistingReport_CorrectSProcReturned()
    {
        var request = new NBi.Core.Report.ReportDataSetRequest(
                string.Empty
                , ReportFileDirectory
                , "Currency_List - SProc"
                , "Currency"
            );

        var parser = new FileReportingParser();
        var query = parser.ExtractCommand(request);

        Assert.That(query.Text,
            Is.EqualTo("usp_CurrencyGetAll"));
        Assert.That(query.CommandType, Is.EqualTo(CommandType.StoredProcedure));
    }
}
