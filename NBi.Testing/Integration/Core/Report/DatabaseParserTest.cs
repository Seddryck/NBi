using System;
using System.Diagnostics;
using System.Linq;
using NBi.Core.Report;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Report
{
    [TestFixture]
    public class DatabaseParserTest
    {

        private static bool isSqlServerStarted = false;

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            isSqlServerStarted = CheckIfSqlServerStarted();
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
            if (!isSqlServerStarted)
                Assert.Ignore("SQL Server not started.");
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }

        private bool CheckIfSqlServerStarted()
        {
            var pname = Process.GetProcesses().Where(p => p.ProcessName.Contains("sqlservr"));
            return pname.Count() > 0;
        }
        #endregion

        [Test]
        [Category("Sql")]
        [Category("ReportServer Database")]
        public void ExtractQuery_ExistingReportAndDataSet_CorrectQueryReturned()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks Sample Reports/"
                    , "Currency_List"
                    ,"Currency"
                );

            var parser = new DatabaseParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query, 
                Is.StringContaining("SELECT").And
                .StringContaining("[CurrencyAlternateKey]").And
                .StringContaining("[AdventureWorksDW2012].[dbo].[DimCurrency]"));
        }

        [Test]
        [Category("Sql")]
        [Category("ReportServer Database")]
        public void ExtractQuery_NonExistingDataSetOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks Sample Reports/"
                    , "Currency_List"
                    , "Non Existing"
                );

            var parser = new DatabaseParser();
            var ex = Assert.Throws<ArgumentException>(()=> parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("'Currency'"));
        }

        [Test]
        [Category("Sql")]
        [Category("ReportServer Database")]
        public void ExtractQuery_NonExistingDataSetMoreThanOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks Sample Reports/"
                    , "Currency Rates"
                    , "Non Existing"
                );

            var parser = new DatabaseParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("DataSet1").And.StringContaining("DataSet2"));
        }

        [Test]
        [Category("Sql")]
        [Category("ReportServer Database")]
        public void ExtractQuery_NonExistingReport_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks Sample Reports/"
                    , "Not Existing"
                    , "DataSet1"
                );

            var parser = new DatabaseParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("No report found"));
        }
    }
}
