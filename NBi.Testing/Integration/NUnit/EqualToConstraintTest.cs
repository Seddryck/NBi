#region Using directives
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBiNu = NBi.NUnit;
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
        public void Matches_MdxQueryAndResulSetWithoutKeyValuesInfo_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2001", "1874469.00" });
            objs.Add(new object[] { "CY 2002", "4511243.0" });
            objs.Add(new object[] { "CY 2003", "4709851" });
            objs.Add(new object[] { "CY 2004", "1513940" });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryAndResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2001", "1874469.00" });
            objs.Add(new object[] { "CY 2002", "4511243.0" });
            objs.Add(new object[] { "CY 2003", "4709851" });
            objs.Add(new object[] { "CY 2004", "1513940" });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);
            ctr.Using(new ResultSetComparaisonSettings(
                    ResultSetComparaisonSettings.KeysChoice.First,
                    ResultSetComparaisonSettings.ValuesChoice.Last,
                    500
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
        public void Matches_MdxQueryAndDecimalResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "CY 2001", 1874469.00 });
            objs.Add(new object[] { "CY 2002", 4511243.0 });
            objs.Add(new object[] { "CY 2003", 4709851 });
            objs.Add(new object[] { "CY 2004", 1513940 });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);
            ctr.Using(new ResultSetComparaisonSettings(
                ResultSetComparaisonSettings.KeysChoice.First,
                ResultSetComparaisonSettings.ValuesChoice.Last,
                null)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryAndSameQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(
                ResultSetComparaisonSettings.KeysChoice.First,
                ResultSetComparaisonSettings.ValuesChoice.Last,
                null)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettings_NotMatching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(
                ResultSetComparaisonSettings.KeysChoice.First,
                ResultSetComparaisonSettings.ValuesChoice.Last,
                null)
            );

            var query = "SELECT [Measures].[Amount] ON 0, ([Date].[Calendar].[Calendar Year]-[Date].[Calendar].[Calendar Year].&[2006]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

        [Test]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettingsAndTolerance_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, ([Date].[Calendar].[Calendar Year]-[Date].[Calendar].[Calendar Year].&[2006]) ON 1  FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(
                    ResultSetComparaisonSettings.KeysChoice.First,
                    ResultSetComparaisonSettings.ValuesChoice.Last,
                    new List<IColumn>()
                    {
                        new Column()
                        {
                            Index=1,
                            Role= ColumnRole.Value,
                            Type=ColumnType.Numeric,
                            Tolerance= 10
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
        public void Matches_MdxQueryAndSqlQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2001',  1874469 UNION ";
            expectedQuery += " SELECT 'CY 2002', 4511243 UNION ";
            expectedQuery += " SELECT 'CY 2003', 4709851 UNION ";
            expectedQuery += " SELECT 'CY 2004', 1513940  ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(
                    new ResultSetComparaisonSettings(
                        ResultSetComparaisonSettings.KeysChoice.AllExpectLast,
                        ResultSetComparaisonSettings.ValuesChoice.Last,
                        new List<IColumn>()
                        {
                            new Column()
                            {
                                Index = 1,
                                Role = ColumnRole.Value,
                                Type = ColumnType.Numeric,
                                Tolerance = 5
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
        public void Matches_MdxQueryAndResulSetCsvFile_Matching()
        {
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("NonEmptyAmountByYear.csv", "NBi.Testing.Integration.NUnit.Resources.NonEmptyAmountByYear.csv");

            var ctr = new NBiNu.EqualToConstraint(filename);

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryWithNullComparedToSqlWithNull_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2006',  NULL ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(
                    new ResultSetComparaisonSettings(
                        ResultSetComparaisonSettings.KeysChoice.AllExpectLast,
                        ResultSetComparaisonSettings.ValuesChoice.Last,
                        null
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2006] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryWithNullComparedToSqlWithValue_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2006',  0 ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(
                    new ResultSetComparaisonSettings(
                        ResultSetComparaisonSettings.KeysChoice.AllExpectLast,
                        ResultSetComparaisonSettings.ValuesChoice.Last,
                        null
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2006] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.False);
        }

    }
}
