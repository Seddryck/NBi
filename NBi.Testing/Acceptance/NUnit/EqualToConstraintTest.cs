#region Using directives
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBiNu = NBi.NUnit;
#endregion

namespace NBi.Testing.Acceptance.NUnit
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
        public void Matches_MdxQuery_Unknown()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "2009", "4242.79" });
            objs.Add(new object[] { "2010", "3845.43" });
            objs.Add(new object[] { "2011", "789.05" });
            objs.Add(new object[] { "2012", "-3795.83" });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

        [Test]
        public void Matches_MdxQueryAndResulSetWithoutKeyValuesInfo_Matching()
        {
            //Buiding object used during test
            var rs = new ResultSet();
            var objs = new List<object[]>();
            objs.Add(new object[] { "2009", "4242.79" });
            objs.Add(new object[] { "2010", "3845.43" });
            objs.Add(new object[] { "2011", "789.05" });
            objs.Add(new object[] { "2012", "-3795.83" });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            objs.Add(new object[] { "2009", "4242.79" });
            objs.Add(new object[] { "2010", "3845.43" });
            objs.Add(new object[] { "2011", "1189.05" });
            objs.Add(new object[] { "2012", "-3495.83" });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);
            ctr.Using(new ResultSetComparaisonSettings(1,1,500));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            objs.Add(new object[] { "2009", 4242.79 });
            objs.Add(new object[] { "2010", 3845.43 });
            objs.Add(new object[] { "2011", 789.05 });
            objs.Add(new object[] { "2012", -3795.83 });
            rs.Load(objs);

            var ctr = new NBiNu.EqualToConstraint(rs);
            ctr.Using(new ResultSetComparaisonSettings(1, 1, 0));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            var expectedQuery = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(1, 1, 0));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            expectedQuery += " SELECT [Measures].NewAmount ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(1, 1, 0));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            expectedQuery += " SELECT [Measures].NewAmount ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(1, 1, 5));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
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
            var expectedQuery = "SELECT '2009',  4242.79 UNION ";
            expectedQuery += " SELECT '2010', 3845.43 UNION ";
            expectedQuery += " SELECT '2011', 789.05 UNION ";
            expectedQuery += " SELECT '2012', -3795.83  ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new NBiNu.EqualToConstraint(expectedCmd);
            ctr.Using(new ResultSetComparaisonSettings(1, 1, 5));

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            //Call the method to test
            var actual = ctr.Matches(cmd);

            //Assertion
            Assert.That(actual, Is.True);
        }

    }
}
