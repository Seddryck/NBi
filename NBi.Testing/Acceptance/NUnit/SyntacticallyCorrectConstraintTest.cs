using System.Data.SqlClient;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.NUnit
{
    [TestFixture]
    public class SyntacticallyCorrectConstraintTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        //TODO Move to acceptance testing
        [Test]
        public void QueryParserRealImplementation_SyntacticallyCorrectConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var cmd = new SqlCommand(sql, conn);


            //Method under test
            Assert.That(cmd, new SyntacticallyCorrectConstraint());

            //Test conclusion            
            Assert.Pass();
        }

        //TODO Move to acceptance testing
        [Test]
        public void QueryParserRealImplementation_IsSyntacticallyCorrect_Success()
        {
            var sql = "SELECT * FROM Product;";
            var conn = new SqlConnection(ConnectionStringReader.GetSqlClient());
            var cmd = new SqlCommand(sql, conn);

            Assert.That(cmd, NBi.NUnit.Is.SyntacticallyCorrect());
            
            Assert.Pass();
        }

    }
}
