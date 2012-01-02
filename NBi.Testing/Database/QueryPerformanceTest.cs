using System.Xml.Schema;
using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Database
{
    [TestFixture]
    public class QueryPerformanceTest
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            _connectionString = "Data Source=.;Initial Catalog=NBi.Testing;Integrated Security=True";
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void TimeLimit_LessThan5000MilliSeconds_Success()
        {
            var sql = "SELECT * FROM Product;";

            var qp = new QueryPerformance(_connectionString, sql);
            var res = qp.Validate(5000);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
        }

        [Test]
        public void TimeLimit_LessThan0MilliSeconds_Failed()
        {
            var sql = "SELECT * FROM Product;";

            var qp = new QueryPerformance(_connectionString, sql);
            var res = qp.Validate(0);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.StringStarting("Maximum time specified was 0"));
        }
    }
}
