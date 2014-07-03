#region Using directives
using System.Data.SqlClient;
using NBi.NUnit.Query;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Query
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

        [Test, Category("Sql")]
        public void Matches_FasterThanMaxTime_Success()
        {
            var command = new SqlCommand();
            command.Connection=new SqlConnection(ConnectionStringReader.GetSqlClient());
            command.CommandText = "WAITFOR DELAY '00:00:00';";

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(1000);
            ctr = ctr.TimeOutMilliSeconds(2000);

            //Method under test
            Assert.That(command, ctr);

        }

        [Test, Category("Sql")]
        public void Matches_SlowerThanMaxTime_Failure()
        {
            var command = new SqlCommand();
            command.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command.CommandText = "WAITFOR DELAY '00:00:01';";

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(100);
            ctr = ctr.TimeOutMilliSeconds(5000);

            //Method under test
            Assert.That(ctr.Matches(command), Is.False);
            //Error Message
        }

        [Test, Category("Sql")]
        public void Matches_SlowerThanMaxTimeAndTimeOut_Failure()
        {
            var command = new SqlCommand();
            command.Connection = new SqlConnection(ConnectionStringReader.GetSqlClient());
            command.CommandText = "WAITFOR DELAY '00:00:10';";

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(100);
            ctr = ctr.TimeOutMilliSeconds(1000);

            //Method under test
            Assert.That(ctr.Matches(command), Is.False);
        }
    }

}
