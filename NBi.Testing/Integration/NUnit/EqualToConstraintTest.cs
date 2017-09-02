#region Using directives
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit
{
    [TestFixture]
    public class EqualToConstraintTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

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


        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetWithoutKeyValuesInfo_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2005", "1874469.00" });
            objs.Add(new object[] { "CY 2006", "4511243.0" });
            objs.Add(new object[] { "CY 2007", "4709851" });
            objs.Add(new object[] { "CY 2008", "1513940" });
            rs.Load(objs);

            var ctr = new EqualToConstraint(rs);

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2005", "1874469.00" });
            objs.Add(new object[] { "CY 2006", "4511243.0" });
            objs.Add(new object[] { "CY 2007", "4709851" });
            objs.Add(new object[] { "CY 2008", "1513940" });
            rs.Load(objs);

            var ctr = new EqualToConstraint(rs);
            ctr.Using(new SettingsResultSetComparisonByIndex(
                    SettingsResultSetComparisonByIndex.KeysChoice.First,
                    SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                    new NumericAbsoluteTolerance(500, SideTolerance.Both)
                )
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndDecimalResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2005", 1874469.00 });
            objs.Add(new object[] { "CY 2006", 4511243.0 });
            objs.Add(new object[] { "CY 2007", 4709851 });
            objs.Add(new object[] { "CY 2008", 1513940 });
            rs.Load(objs);

            var ctr = new EqualToConstraint(rs);
            ctr.Using(new SettingsResultSetComparisonByIndex(
                SettingsResultSetComparisonByIndex.KeysChoice.First,
                SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSameQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(new SettingsResultSetComparisonByIndex(
                SettingsResultSetComparisonByIndex.KeysChoice.First,
                SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettings_NotMatching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(new SettingsResultSetComparisonByIndex(
                SettingsResultSetComparisonByIndex.KeysChoice.First,
                SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, ([Date].[Calendar].[Calendar Year]-[Date].[Calendar].[Calendar Year].&[2010]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettingsAndTolerance_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, ([Date].[Calendar].[Calendar Year].[CY 2005]:[Date].[Calendar].[Calendar Year].[CY 2008]) ON 1  FROM [Adventure Works]";
            var expectedCmd   = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(new SettingsResultSetComparisonByIndex(
                    SettingsResultSetComparisonByIndex.KeysChoice.First,
                    SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                    new List<IColumnDefinition>()
                    {
                        new Column()
                        {
                            Index=1,
                            Role= ColumnRole.Value,
                            Type=ColumnType.Numeric,
                            Tolerance= "10"
                        }
                    }
                )
            );

            var query = "SELECT  [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Sql")]
        [Category("Olap")]
        public void Matches_MdxQueryAndSqlQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2005',  1874469 UNION ";
            expectedQuery += " SELECT 'CY 2006', 4511243 UNION ";
            expectedQuery += " SELECT 'CY 2007', 4709851 UNION ";
            expectedQuery += " SELECT 'CY 2008', 1513940  ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        new List<IColumnDefinition>()
                        {
                            new Column()
                            {
                                Index = 1,
                                Role = ColumnRole.Value,
                                Type = ColumnType.Numeric,
                                Tolerance = "5"
                            }
                        }
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetCsvFile_Matching()
        {
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("NonEmptyAmountByYear.csv", "NBi.Testing.Integration.NUnit.Resources.NonEmptyAmountByYear.csv");

            var ctr = new EqualToConstraint(filename);

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithNull_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  NULL ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithValue_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  0 ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToString_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>();
            columns.Add(new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime });

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        columns
                    )
                );

            var query = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherDate_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-02' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>();
            columns.Add(new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime });

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        columns
                    )
                );

            var query = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherHour_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>();
            columns.Add(new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime });

            var ctr = new EqualToConstraint(expectedCmd);
            ctr.Using(
                    new SettingsResultSetComparisonByIndex(
                        SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast,
                        SettingsResultSetComparisonByIndex.ValuesChoice.Last,
                        columns
                    )
                );

            var query = "SELECT 'CY 2010',  '1/01/2010 01:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

    }
}
