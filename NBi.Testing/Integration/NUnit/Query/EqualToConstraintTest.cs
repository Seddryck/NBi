#region Using directives
using System;
using System.Data.SqlClient;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using System.Data;
using NBi.Core.Injection;
using System.Collections.Generic;
using NBi.Core.Scalar.Comparer;
using NBi.Core;
using NBi.Extensibility.Query;
using NBi.Core.Scalar.Resolver;
using NBi.Core.FlatFile;
#endregion

namespace NBi.Testing.Integration.NUnit.Query
{
    [TestFixture]
    public class EqualToConstraintTest
    {
        private readonly ServiceLocator serviceLocator = new ServiceLocator();

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

        private class FakeQueryResultSetResolver : QueryResultSetResolver
        {
            private readonly IQuery query;

            public FakeQueryResultSetResolver(IQuery query, ServiceLocator serviceLocator)
                : base(null, serviceLocator)
            {
                this.query = query;
            }

            protected override IQuery Resolve() => query;
        }

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsParallel_FasterThanSixSeconds()
        {
            var query1 = new NBi.Core.Query.Query("WAITFOR DELAY '00:00:03';SELECT 1;", ConnectionStringReader.GetSqlClient());
            var query2 = new NBi.Core.Query.Query("WAITFOR DELAY '00:00:03';SELECT 1;", ConnectionStringReader.GetSqlClient());

            var resolver = new FakeQueryResultSetResolver(query2, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Parallel();

            //Method under test
            var chrono = DateTime.Now;
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query1, serviceLocator));
            var actual = actualBuilder.GetService();
            Assert.That(actual, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.LessThan(6));
        }

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsSequential_SlowerThanSixSeconds()
        {
            var query1 = new NBi.Core.Query.Query("WAITFOR DELAY '00:00:03';SELECT 1;", ConnectionStringReader.GetSqlClient());
            var query2 = new NBi.Core.Query.Query("WAITFOR DELAY '00:00:03';SELECT 1;", ConnectionStringReader.GetSqlClient());

            var loader = new FakeQueryResultSetResolver(query2, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Sequential();

            //Method under test
            var chrono = DateTime.Now;
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query1, serviceLocator));
            var actual = actualBuilder.GetService();

            Assert.That(actual, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.GreaterThanOrEqualTo(6));
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

            var resolver = new ObjectsResultSetResolver(new ObjectsResultSetResolverArgs(objs.ToArray()));
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());

            var mdx = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
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

            var resolver = new ObjectsResultSetResolver(new ObjectsResultSetResolverArgs(objs.ToArray()));
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsOrdinalResultSet(
                    SettingsOrdinalResultSet.KeysChoice.First,
                    SettingsOrdinalResultSet.ValuesChoice.Last,
                    new NumericAbsoluteTolerance(500, SideTolerance.Both)
                )
            );

            var mdx = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
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
            var resolver = new ObjectsResultSetResolver(new ObjectsResultSetResolverArgs(objs.ToArray()));
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsOrdinalResultSet(
                SettingsOrdinalResultSet.KeysChoice.First,
                SettingsOrdinalResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var mdx = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSameQueryWithCorrectSettings_Matching()
        {
            //Buiding object used during test
            var mdx = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedQuery = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsOrdinalResultSet(
                SettingsOrdinalResultSet.KeysChoice.First,
                SettingsOrdinalResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var mdx2 = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1  FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx2, ConnectionStringReader.GetAdomd());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettings_NotMatching()
        {
            //Buiding object used during test
            var mdx = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            mdx += " SELECT [Measures].NewAmount ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var expectedQuery = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsOrdinalResultSet(
                SettingsOrdinalResultSet.KeysChoice.First,
                SettingsOrdinalResultSet.ValuesChoice.Last,
                NumericAbsoluteTolerance.None)
            );

            var mdx2 = "SELECT [Measures].[Amount] ON 0, ([Date].[Calendar].[Calendar Year]-[Date].[Calendar].[Calendar Year].&[2010]) ON 1  FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx2, ConnectionStringReader.GetAdomd());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryAndSlighltyDifferentQueryWithCorrectSettingsAndTolerance_Matching()
        {
            //Buiding object used during test
            var mdx = "WITH MEMBER [Measures].NewAmount AS [Measures].[Amount]+1";
            mdx += " SELECT [Measures].NewAmount ON 0, ([Date].[Calendar].[Calendar Year].[CY 2005]:[Date].[Calendar].[Calendar Year].[CY 2008]) ON 1  FROM [Adventure Works]";
            var expectedQuery = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(new SettingsOrdinalResultSet(
                    SettingsOrdinalResultSet.KeysChoice.First,
                    SettingsOrdinalResultSet.ValuesChoice.Last,
                    new List<IColumnDefinition>()
                    {
                        new Column()
                        {
                            Identifier= new ColumnOrdinalIdentifier(1),
                            Role= ColumnRole.Value,
                            Type=ColumnType.Numeric,
                            Tolerance= "10"
                        }
                    }
                )
            );

            var query = "SELECT  [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var cmd = new NBi.Core.Query.Query(query, ConnectionStringReader.GetAdomd());


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(cmd, serviceLocator));
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
            var sql = "SELECT 'CY 2005',  1874469 UNION ";
            sql += " SELECT 'CY 2006', 4511243 UNION ";
            sql += " SELECT 'CY 2007', 4709851 UNION ";
            sql += " SELECT 'CY 2008', 1513940  ";

            var expectedQuery = new NBi.Core.Query.Query(sql, ConnectionStringReader.GetSqlClient());
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsOrdinalResultSet(
                        SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                        SettingsOrdinalResultSet.ValuesChoice.Last,
                        new List<IColumnDefinition>()
                        {
                            new Column()
                            {
                                Identifier= new ColumnOrdinalIdentifier(1),
                                Role = ColumnRole.Value,
                                Type = ColumnType.Numeric,
                                Tolerance = "5"
                            }
                        }
                    )
                );

            var mdx = "SELECT  [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
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
            var resolver = new FlatFileResultSetResolver(new FlatFileResultSetResolverArgs(new LiteralScalarResolver<string>(filename), string.Empty, string.Empty, null, CsvProfile.SemiColumnDoubleQuote), serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());

            var mdx = "SELECT [Measures].[Amount] ON 0, NON EMPTY([Date].[Calendar].[Calendar Year]) ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());


            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithNull_Matching()
        {
            var sql = "SELECT 'CY 2010',  NULL ";
            var expectedQuery = new NBi.Core.Query.Query(sql, ConnectionStringReader.GetSqlClient());

            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsOrdinalResultSet(
                        SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                        SettingsOrdinalResultSet.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var mdx = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Olap")]
        public void Matches_MdxQueryWithNullComparedToSqlWithValue_NonMatching()
        {
            //Buiding object used during test
            var sql = "SELECT 'CY 2010',  0 ";
            var expectedQuery = new NBi.Core.Query.Query(sql, ConnectionStringReader.GetSqlClient());
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsOrdinalResultSet(
                        SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                        SettingsOrdinalResultSet.ValuesChoice.Last,
                        NumericAbsoluteTolerance.None
                    )
                );

            var mdx = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var query = new NBi.Core.Query.Query(mdx, ConnectionStringReader.GetAdomd());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToString_Matching()
        {
            var expectedSql = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";
            var expectedQuery = new NBi.Core.Query.Query(expectedSql, ConnectionStringReader.GetSqlClient());

            var columns = new List<IColumnDefinition>(){
                new Column() { Identifier= new ColumnOrdinalIdentifier(1), Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsOrdinalResultSet(
                        SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                        SettingsOrdinalResultSet.ValuesChoice.Last,
                        columns
                    )
                );

            var sql = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var query = new NBi.Core.Query.Query(sql, ConnectionStringReader.GetSqlClient());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual));
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherDate_NonMatching()
        {
            //Buiding object used during test
            var expectedSql = "SELECT 'CY 2010',  CAST('2010-01-02' AS DATE)";
            var expectedQuery = new NBi.Core.Query.Query(expectedSql, ConnectionStringReader.GetSqlClient());

            var columns = new List<IColumnDefinition>(){
                new Column() { Identifier= new ColumnOrdinalIdentifier(1), Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };
            var resolver = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var ctr = new EqualToConstraint(builder.GetService());
            ctr.Using(
                    new SettingsOrdinalResultSet(
                        SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                        SettingsOrdinalResultSet.ValuesChoice.Last,
                        columns
                    )
                );

            var sql2 = "SELECT 'CY 2010',  '1/01/2010 00:00:00'";
            var query = new NBi.Core.Query.Query(sql2, ConnectionStringReader.GetSqlClient());

            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = actualBuilder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }

        [Test]
        [Category("Sql")]
        public void Matches_SqlQueryWithDateComparedToStringAnotherHour_NonMatching()
        {
            //Buiding object used during test
            var expectedSql = "SELECT 'CY 2010',  CAST('2010-01-01' AS DATE)";
            var expectedQuery = new NBi.Core.Query.Query(expectedSql, ConnectionStringReader.GetSqlClient());

            var columns = new List<IColumnDefinition>(){
                new Column() { Identifier= new ColumnOrdinalIdentifier(1), Role = ColumnRole.Value, Type = ColumnType.DateTime }
            };

            var expectedLoader = new FakeQueryResultSetResolver(expectedQuery, serviceLocator);
            var expectedBuilder = new ResultSetServiceBuilder();
            expectedBuilder.Setup(expectedLoader);
            var ctr = new EqualToConstraint(expectedBuilder.GetService());
            ctr.Using(
                                new SettingsOrdinalResultSet(
                                    SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                                    SettingsOrdinalResultSet.ValuesChoice.Last,
                                    columns
                                )
                            );

            var sql2 = "SELECT 'CY 2010',  '1/01/2010 01:00:00'";
            var query = new NBi.Core.Query.Query(sql2, ConnectionStringReader.GetSqlClient());
            var builder = new ResultSetServiceBuilder();
            builder.Setup(new FakeQueryResultSetResolver(query, serviceLocator));
            var actual = builder.GetService();

            //Assertion
            Assert.That(ctr.Matches(actual), Is.False);
        }
    }
}
