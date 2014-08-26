﻿using System;
using System.Diagnostics;
using System.Linq;
using NBi.Core.Report;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Report
{
    [TestFixture]
    [Category("ReportServerDB")]
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
                .StringContaining("[DimCurrency]"));
        }

        [Test]
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
            Assert.That(ex.Message, Is.StringContaining("'Currency_List'"));
        }

        [Test]
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

        public void ExtractQuery_SharedDataSet_CorrectQuery()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks 2012/"
                    , "Employee_Sales_Summary"
                    , "SalesEmployees2008R2"
                );

            var parser = new DatabaseParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query,
                Is.StringContaining("SELECT"));
        }

        public void ExtractQuery_NonExistingSharedDataSet_CorrectQuery()
        {
            var request = new NBi.Core.Report.DatabaseRequest(
                    ConnectionStringReader.GetReportServerDatabase()
                    , "/AdventureWorks 2012/"
                    , "Employee_Sales_Summary"
                    , "NonExisting"
                );

            var parser = new DatabaseParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("Quota").And.StringContaining("2008R2"));
        }
    }
}
