using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core.Report;
using NUnit.Framework;
using System.Data;

namespace NBi.Testing.Unit.Core.Report
{
    [TestFixture]
    public class FileParserTest
    {

        private string ReportFileDirectory { get; set; }

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
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
        [TestFixtureTearDown]
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
            var resource = "NBi.Testing.Unit.Core.Report.Resources." + filename + ".rdl";
            var physicalFilename = DiskOnFile.CreatePhysicalFile(file, resource);
            ReportFileDirectory = Path.GetDirectoryName(physicalFilename) + Path.DirectorySeparatorChar.ToString();
        }

        protected void CreateSharedDataSet(string filename)
        {
            string file = @"\Temp\" + filename + ".rsd";
            var resource = "NBi.Testing.Unit.Core.Report.Resources." + filename + ".rsd";
            var physicalFilename = DiskOnFile.CreatePhysicalFile(file, resource);
            ReportFileDirectory = Path.GetDirectoryName(physicalFilename) + Path.DirectorySeparatorChar.ToString();
        }

        [Test]
        public void ExtractQuery_ExistingReportAndDataSet_CorrectQueryReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Currency_List"
                    , "Currency"
                );

            var parser = new FileParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query.Text,
                Is.StringContaining("SELECT").And
                .StringContaining("[CurrencyAlternateKey]").And
                .StringContaining("[DimCurrency]"));
            Assert.That(query.CommandType, Is.EqualTo(CommandType.Text));
        }

        [Test]
        public void ExtractQuery_NonExistingDataSetOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Currency_List"
                    , "Non Existing"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("'Currency'"));
        }

        [Test]
        public void ExtractQuery_NonExistingDataSetMoreThanOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Currency_Rates"
                    , "Non Existing"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("DataSet1").And.StringContaining("DataSet2"));
        }

        [Test]
        public void ExtractQuery_NonExistingReport_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Not Existing"
                    , "DataSet1"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("No report found"));
        }

        [Test]
        public void ExtractQuery_ExistingReportAndSharedDataSet_CorrectQueryReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Employee_Sales_Summary"
                    , "SalesEmployees2008R2"
                );

            var parser = new FileParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query.Text,
                Is.StringContaining("SELECT").And
                .StringContaining("[Sales].[SalesPerson]").And
                .StringContaining("[HumanResources].[Employee]"));
            Assert.That(query.CommandType, Is.EqualTo(CommandType.Text));
        }

        [Test]
        public void ExtractSProc_ExistingReport_CorrectSProcReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    ReportFileDirectory
                    , "Currency_List - SProc"
                    , "Currency"
                );

            var parser = new FileParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query.Text,
                Is.EqualTo("usp_CurrencyGetAll"));
            Assert.That(query.CommandType, Is.EqualTo(CommandType.StoredProcedure));
        }
    }
}
