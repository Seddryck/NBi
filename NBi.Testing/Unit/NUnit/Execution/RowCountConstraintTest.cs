using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Execution;
using NUnit.Framework;
using NBi.Core.ResultSet;
using System.Data.SqlClient;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.Testing.Unit.NUnit.Execution
{
    [TestFixture]
    public class RowCountConstraintTest
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
        public void Matches_SqlCommand_CallToResultSetBuilderOnce()
        {
            var resultSet = new ResultSet();
            resultSet.Load("a;b;c");
            var cmd = new SqlCommand();

            var rsbMock = new Mock<ResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(It.IsAny<object>()))
                .Returns(resultSet);
            var rsb = rsbMock.Object;

            var child = new NUnitCtr.GreaterThanConstraint(0);

            var rowCount = new RowCountConstraint(child) { ResultSetBuilder = rsb };
            rowCount.ResultSetBuilder = rsb;

            //Method under test
            rowCount.Matches(cmd);

            //Test conclusion            
            rsbMock.Verify(engine => engine.Build(It.IsAny<object>()), Times.Once());
        }

    }
}
