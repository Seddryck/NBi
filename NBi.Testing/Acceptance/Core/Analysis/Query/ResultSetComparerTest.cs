using System.Data.SqlClient;
using NBi.Core.Analysis.Query;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Analysis.Query
{
    [TestFixture]
    public class ResultSetComparerTest
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
        public void Validate_SameQuerySameDatabase_ReturnSuccess()
        {
            var connActual = new SqlConnection(ConnectionStringReader.Get());
            var cmdActual = new SqlCommand("SELECT * FROM Product;", connActual);

            var connExpect = new SqlConnection(ConnectionStringReader.Get());
            var cmdExpect = new SqlCommand("SELECT * FROM Product;", connExpect);

            var rsc = new ResultSetComparer(cmdExpect, string.Empty, string.Empty);

            var res = rsc.Validate(cmdActual);

            Assert.That(res.Value, Is.EqualTo(NBi.Core.Result.Success().Value));

        }

        [Test, Category("Sql database")]
        public void Validate_QueriesReturningDifferentResults_ReturnFailed()
        {
            var connActual = new SqlConnection(ConnectionStringReader.Get());
            var cmdActual = new SqlCommand("SELECT TOP 1 * FROM Product;", connActual);

            var connExpect = new SqlConnection(ConnectionStringReader.Get());
            var cmdExpect = new SqlCommand("SELECT * FROM Product;", connExpect);

            var rsc = new ResultSetComparer(cmdExpect, string.Empty, string.Empty);

            var res = rsc.Validate(cmdActual);

            Assert.That(res.Value, Is.EqualTo(NBi.Core.Result.Failed().Value));

        }
    }
}
