using System.IO;
using System.Diagnostics;
using System.Reflection;
using SysConfig = System.Configuration;
using System.Collections.Generic;
using NUnit.Framework;
using System;

namespace NBi.Testing.Acceptance
{
    [TestFixture]
    public class RuntimeOverrider : BaseRuntimeOverrider
    {

        [TestFixtureSetUp]
        public void SetupMethods()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            DiskOnFile.CreatePhysicalFile(@"Etl\Sample.dtsx", "NBi.Testing.Integration.SqlServer.IntegrationService.Resources.Sample.dtsx");

            //Set environment variable
            Environment.SetEnvironmentVariable("FirstJanuary2015", "2015-01-01", EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("ConnStrAdvWorksCloud", ConnectionStringReader.GetSqlClient(), EnvironmentVariableTarget.User);
        }

        [TestFixtureTearDown]
        public void TearDownMethods()
        {
            //Delete environment variable
            Environment.SetEnvironmentVariable("FirstJanuary2015", null, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("ConnStrAdvWorksCloud", null, EnvironmentVariableTarget.User);

        }

        //By Acceptance Test Suite (file) create a Test Case
        [Test]
        //[TestCase("QueryUniqueRows.nbits")]
        //[TestCase("AssemblyEqualToResultSet.nbits")]
        //[TestCase("CsvEqualToResultSet.nbits")]
        //[TestCase("QueryEqualToWithParameter.nbits")]
        //[TestCase("QueryEqualToCsv.nbits")]
        //[TestCase("QueryEqualToCsvWithProfile.nbits")]
        //[TestCase("QueryEqualToQuery.nbits")]
        //[TestCase("QuerySubsetOfQuery.nbits")]
        //[TestCase("QuerySupersetOfQuery.nbits")]
        //[TestCase("QueryEqualToResultSet.nbits")]
        //[TestCase("QueryEqualToResultSetWithNull.nbits")]
        //[TestCase("QueryWithReference.nbits")]
        //[TestCase("Ordered.nbits")]
        //[TestCase("Count.nbits")]
        //[TestCase("Contain.nbits")]
        //[TestCase("ContainStructure.nbits")]
        //[TestCase("FasterThan.nbits")]
        //[TestCase("SyntacticallyCorrect.nbits")]
        //[TestCase("Exists.nbits")]
        //[TestCase("LinkedTo.nbits")]
        //[TestCase("SubsetOfStructure.nbits")]
        //[TestCase("EquivalentToStructure.nbits")]
        //[TestCase("SubsetOfMembers.nbits")]
        //[TestCase("EquivalentToMembers.nbits")]
        //[TestCase("MatchPatternMembers.nbits")]
        //[TestCase("ResultSetMatchPattern.nbits")]
        //[TestCase("QueryWithParameters.nbits")]
        //[TestCase("ReportEqualTo.nbits")]
        //[TestCase("Etl.nbits")]
        //[TestCase("Decoration.nbits")]
        //[TestCase("Is.nbits")]
        //[TestCase("QueryEqualToXml.nbits")]
        //[TestCase("QueryRowCount.nbits")]
        //[TestCase("QueryAllNoRows.nbits")]
        [TestCase("ResultSetConstraint.nbits")]
        //[TestCase("Scoring.nbits")]
        //[TestCase("Environment.nbits")]
        //[TestCase("MultipleInstance.nbits")]
        //[TestCase("PowerBiDesktop.nbits")]
        //[TestCase("EvaluateRows.nbits")]
        [Category("Acceptance")]
        public override void RunPositiveTestSuite(string filename)
        {
            base.RunPositiveTestSuite(filename);
        }

        [Test]
        [TestCase("QueryEqualToResultSetProvider.nbits")]
        [TestCase("Variable.nbits")]
        [Category("Acceptance")]
        public override void RunPositiveTestSuiteWithConfig(string filename)
        {
            base.RunPositiveTestSuiteWithConfig(filename);
        }

        [Test]
        [TestCase("DataRowsMessage.nbits")]
        [TestCase("ItemsMessage.nbits")]
        [Category("Acceptance")]
        public override void RunNegativeTestSuite(string filename)
        {
            base.RunNegativeTestSuite(filename);
        }

        [Test]
        //[TestCase("Config-Full-Json.nbits")]
        //[TestCase("Config-Full.nbits")]
        //[TestCase("Config-Light.nbits")]
        [TestCase("Scoring-Json.nbits")]
        [Category("Acceptance")]
        public override void RunNegativeTestSuiteWithConfig(string filename)
        {
            base.RunNegativeTestSuiteWithConfig(filename);
        }

        [Test]
        [TestCase("Ignored.nbits")]
        public override void RunIgnoredTests(string filename)
        {
            base.RunIgnoredTests(filename);
        }
    }
}
