#region Using directives
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core.ResultSet.Resolver;
using NBi.Core;
using System.Data;
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

        private class FakeQueryResultSetLoader : QueryResultSetResolver
        {
            private readonly IDbCommand cmd;

            public FakeQueryResultSetLoader(IDbCommand cmd)
                : base(null)
            {
                this.cmd = cmd;
            }

            protected override IDbCommand Resolve()
            {
                return cmd;
            }
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetWithoutKeyValuesInfo_Matching()
        {
            //Buiding object used during test
            var objs = new List<object[]>(){
                new object[] { "CY 2005", "1874469.00" },
                new object[] { "CY 2006", "4511243.0" },
                new object[] { "CY 2007", "4709851" },
                new object[] { "CY 2008", "1513940" }
            };

            var loader = new ObjectArrayResultSetResolver(objs.ToArray());
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var objs = new List<object[]>(){
                new object[] { "CY 2005", "1874469.00" },
                new object[] { "CY 2006", "4511243.0" },
                new object[] { "CY 2007", "4709851" },
                new object[] { "CY 2008", "1513940" }
            };

            var loader = new ObjectArrayResultSetResolver(objs.ToArray());
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsIndexResultSet(
                    SettingsIndexResultSet.KeysChoice.First,
                    SettingsIndexResultSet.ValuesChoice.Last,
                    new NumericAbsoluteTolerance(500, SideTolerance.Both)
                )
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndDecimalResulSetWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var objs = new List<object[]>() {
                new object[] { "CY 2005", 1874469.00 },
                new object[] { "CY 2006", 4511243.0 },
                new object[] { "CY 2007", 4709851 },
                new object[] { "CY 2008", 1513940 }
            };
            var loader = new ObjectArrayResultSetResolver(objs.ToArray());
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsIndexResultSet(
                SettingsIndexResultSet.KeysChoice.First,
                SettingsIndexResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSameQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsIndexResultSet(
                SettingsIndexResultSet.KeysChoice.First,
                SettingsIndexResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettings_NotMatching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsIndexResultSet(
                SettingsIndexResultSet.KeysChoice.First,
                SettingsIndexResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var query = "SELECT [Measures].[Amount] ON 0, ([Date].[Calendar].[Calendar Year]-[Date].[Calendar].[Calendar Year].&[2010]) ON 1  FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettingsAndTolerance_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            expectedQuery += " SELECT [Measures].NewAmount ON 0, ([Date].[Calendar].[Calendar Year].[CY 2005]:[Date].[Calendar].[Calendar Year].[CY 2008]) ON 1  FROM [Adventure Works]";
            var expectedCmd = new AdomdCommand(expectedQuery, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsIndexResultSet(
                    SettingsIndexResultSet.KeysChoice.First,
                    SettingsIndexResultSet.ValuesChoice.Last,
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


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
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
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsIndexResultSet(
                        SettingsIndexResultSet.KeysChoice.AllExpectLast,
                        SettingsIndexResultSet.ValuesChoice.Last,
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


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndResulSetCsvFile_Matching()
        {
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("NonEmptyAmountByYear.csv", "NBi.Testing.Integration.NUnit.Resources.NonEmptyAmountByYear.csv");
            var loader = new CsvResultSetResolver(filename, CsvProfile.SemiColumnDoubleQuote);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());

            var query = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithNull_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  NULL ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsIndexResultSet(
                        SettingsIndexResultSet.KeysChoice.AllExpectLast,
                        SettingsIndexResultSet.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithValue_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  0 ";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsIndexResultSet(
                        SettingsIndexResultSet.KeysChoice.AllExpectLast,
                        SettingsIndexResultSet.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToString_Matching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>(){
                new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsIndexResultSet(
                        SettingsIndexResultSet.KeysChoice.AllExpectLast,
                        SettingsIndexResultSet.ValuesChoice.Last,
                        columns
                    )
                );

            var query = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherDate_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-02' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>(){
                new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };
            var loader = new FakeQueryResultSetLoader(expectedCmd);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsIndexResultSet(
                        SettingsIndexResultSet.KeysChoice.AllExpectLast,
                        SettingsIndexResultSet.ValuesChoice.Last,
                        columns
                    )
                );

            var query = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherHour_NonMatching()
        {
            //Buiding object used during test
            var expectedQuery = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";

            var expectedCmd = new SqlCommand(expectedQuery, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var columns = new List<IColumnDefinition>(){
                new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };

            var expectedLoader = new FakeQueryResultSetLoader(expectedCmd);
            var expectedBuilder = new ResultSetServiceBuilder();
            expectedBuilder.Setup(expectedLoader);
            var ctr = new EqualToConstraint(expectedBuilder.GetService());
            ctr.Using(
                                new SettingsIndexResultSet(
                                    SettingsIndexResultSet.KeysChoice.AllExpectLast,
                                    SettingsIndexResultSet.ValuesChoice.Last,
                                    columns
                                )
                            );

            var query = "SELECT 'CY 2010',  '1/01/2010 01:00:00'";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var builder = new ResultSetServiceBuilder();
            builder.Setup(new FakeQueryResultSetLoader(cmd));
            var actual = builder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

    }
}
