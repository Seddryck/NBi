#region Using directives
using System.Data.SqlClient;
using NBi.NUnit.Query;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit
{
    [TestFixture]
    public class FasterThanConstraintTest
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

        [Test, Category("Sql database")]
        public void QueryPerformanceRealImplementation_FasterThanConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(5000);
            ctr = ctr.CleanCache();

            //Method under test
            Assert.That(cmd, ctr);

            //Test conclusion            
            Assert.Pass();
        }

        [Test, Category("Sql database")]
        public void QueryPerformanceRealImplementation_IsFasterThan_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(5000);
            ctr = ctr.CleanCache();

            //Method under test
            Assert.That(cmd, ctr);

            Assert.Pass();
        }

    }
}
