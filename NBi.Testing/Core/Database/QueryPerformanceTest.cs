using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Core.Database
{
    [TestFixture]
    public class QueryPerformanceTest
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.user.config");
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.config");
            }
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

            var qp = new QueryPerformance(_connectionString,5000);
            var res = qp.Validate(sql);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
        }

        [Test]
        public void TimeLimit_LessThan0MilliSeconds_Failed()
        {
            var sql = "SELECT * FROM Product;";

            var qp = new QueryPerformance(_connectionString, -1);
            var res = qp.Validate(sql);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.StringStarting("Maximum time specified was"));
        }
    }
}
