#region Using directives
using System;
using System.Data.SqlClient;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core.ResultSet.Loading;
using NBi.Core.ResultSet;
using System.Data;
#endregion

namespace NBi.Testing.Integration.NUnit.Query
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

        private class FakeQueryResultSetLoader : QueryResultSetLoader
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

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsParallel_FasterThanSixSeconds()
        {
            var command = new SqlCommand
            {
                Connection = new SqlConnection(ConnectionStringReader.GetSqlClient()),
                CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;"
            };

            var command2 = new SqlCommand
            {
                Connection = new SqlConnection(ConnectionStringReader.GetSqlClient()),
                CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;"
            };

            var loader = new FakeQueryResultSetLoader(command2);
            var builder = new ResultSetServiceBuilder() { Loader = loader };
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Parallel();

            //Method under test
            var chrono = DateTime.Now;
            var actual = new ResultSetServiceBuilder() { Loader = new FakeQueryResultSetLoader(command) }.GetService();
            Assert.That(actual, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.LessThan(6));
        }

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsSequential_SlowerThanSixSeconds()
        {
            var command1 = new SqlCommand
            {
                Connection = new SqlConnection(ConnectionStringReader.GetSqlClient()),
                CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;"
            };

            var command2 = new SqlCommand
            {
                Connection = new SqlConnection(ConnectionStringReader.GetSqlClient()),
                CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;"
            };

            var loader = new FakeQueryResultSetLoader(command2);
            var builder = new ResultSetServiceBuilder() { Loader = loader };
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Sequential();

            //Method under test
            var chrono = DateTime.Now;
            var actual = new ResultSetServiceBuilder() { Loader = new FakeQueryResultSetLoader(command1) }.GetService();
            Assert.That(actual, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.GreaterThanOrEqualTo(6));
        }
    }
}
