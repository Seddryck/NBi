using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Database
{
    [TestFixture]
    public class DataSetConstraintTest
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
        public void DataSetRealImplementation_DataSetConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";

            //Method under test
            Assert.That(sql, new DataSetConstraint(_connectionString, _connectionString));

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

        [Test]
        public void DataSetMock_IsSameThan_CalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IDataSetComparer>();

            mock.Setup(engine => engine.Validate(sql))
                .Returns(Result.Success());
            IDataSetComparer dsc = mock.Object;

            var dsConstraint = new DataSetConstraint(dsc);

            //Method under test
            Assert.That(sql, dsConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(sql), Times.AtMostOnce());
        }

        [Test]
        public void DataSetMock_IsSameStructureThan_CalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IDataSetComparer>();

            mock.Setup(engine => engine.ValidateStructure(sql))
                .Returns(Result.Success());
            IDataSetComparer dsc = mock.Object;

            var dsConstraint = new DataSetStructureConstraint(dsc);

            //Method under test
            Assert.That(sql, dsConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.ValidateStructure(sql), Times.AtMostOnce());
        }

        [Test]
        public void DataSetMock_IsSameContentThan_CalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IDataSetComparer>();

            mock.Setup(engine => engine.ValidateContent(sql))
                .Returns(Result.Success());
            IDataSetComparer dsc = mock.Object;

            var dsConstraint = new DataSetContentConstraint(dsc);

            //Method under test
            Assert.That(sql, dsConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.ValidateContent(sql), Times.AtMostOnce());
        }

    }
}
