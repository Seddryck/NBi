#region Using directives
using System.Data.SqlClient;
using NBi.NUnit;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Acceptance.NUnit
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

        //TODO Move to acceptance testing
        [Test, Category("Sql database")]
        public void QueryPerformanceRealImplementation_FasterThanConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.Get()));

            //Method under test
            Assert.That(cmd, new FasterThanConstraint(5000, true));

            //Test conclusion            
            Assert.Pass();
        }

        [Test, Category("Sql database")]
        public void QueryPerformanceRealImplementation_IsFasterThan_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.Get()));

            //Method under test
            Assert.That(cmd, NBi.NUnit.Is.FasterThan(5000, true));

            Assert.Pass();
        }

    }
}
