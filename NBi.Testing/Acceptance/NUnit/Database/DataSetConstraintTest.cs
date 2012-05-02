#region Using directives
using NBi.NUnit;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Acceptance.NUnit.Database
{
    [TestFixture]
    public class DataSetConstraintTest
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

        
        [Test]
        public void DataSetRealImplementation_DataSetConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";

            //Method under test
            Assert.That(sql, new DataSetConstraint(ConnectionStringReader.Get(), ConnectionStringReader.Get()));

            //Test conclusion            
            Assert.Pass();
        }

        //[Test]
        //public void DataSetRealImplementation_IsSameStructureThan_Success()
        //{
        //    var sql = "SELECT * FROM Product;";

        //    Assert.That(sql, OnDataSource.Localized(_connectionString).Is.SameStructureThan(_connectionString, sql));

        //    Assert.Pass();
        //}
        

    }
}
