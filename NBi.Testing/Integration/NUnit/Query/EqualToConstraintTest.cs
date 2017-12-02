#region Using directives
using System;
using System.Data.SqlClient;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using System.Data;
using NBi.Core.Query;
using Moq;
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

        private class FakeQueryResultSetLoader : QueryResultSetResolver
        {
            private readonly IQuery query;

            public FakeQueryResultSetLoader(IQuery query)
                : base(null)
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

            var loader = new FakeQueryResultSetLoader(query2);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Parallel();

            //Method under test
            var chrono = DateTime.Now;
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(query1));
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

            var loader = new FakeQueryResultSetLoader(query2);
            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            BaseResultSetComparisonConstraint ctr = new EqualToConstraint(builder.GetService());
            ctr = ctr.Sequential();

            //Method under test
            var chrono = DateTime.Now;
            var actualBuilder = new ResultSetServiceBuilder();
            actualBuilder.Setup(new FakeQueryResultSetLoader(query1));
            var actual = actualBuilder.GetService();

            Assert.That(actual, ctr);
            var elapsed = DateTime.Now.Subtract(chrono);

            Assert.That(elapsed.Seconds, Is.GreaterThanOrEqualTo(6));
        }
    }
}
