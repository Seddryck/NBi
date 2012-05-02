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
