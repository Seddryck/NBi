#region Using directives
using System;
using System.Data.SqlClient;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core.ResultSet.Service;
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

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsParallel_FasterThanSixSeconds()
        {
            var command = new SqlCommand();
            command.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command.CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;";

            var command2 = new SqlCommand();
            command2.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command2.CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;";

            var service = new QueryResultSetService(command2);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(service);
            ctr = ctr.Parallel();

            //Method under test
            var chrono = DateTime.Now;
            Assert.That(command, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.LessThan(6));
        }

        [Test, Category("Sql"), Category("Slow")]
        public void Matches_TwoQueriesOfThreeSecondsSequential_SlowerThanSixSeconds()
        {
            var command = new SqlCommand();
            command.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command.CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;";

            var command2 = new SqlCommand();
            command2.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command2.CommandText = "WAITFOR DELAY '00:00:03';SELECT 1;";

            var service = new QueryResultSetService(command2);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(service);
            ctr = ctr.Sequential();

            //Method under test
            var chrono = DateTime.Now;
            Assert.That(command, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.GreaterThanOrEqualTo(6));
        }
    }
}
