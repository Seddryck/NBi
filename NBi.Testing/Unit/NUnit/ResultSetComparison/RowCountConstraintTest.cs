using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core.ResultSet;
using System.Data.SqlClient;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Resolver;
using NBi.NUnit;
using NUnit.Framework.Constraints;
using NBi.Core.Scalar.Resolver;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
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

            var serviceMock = new Mock<IResultSetService>();
            serviceMock.Setup(s => s.Execute())
                .Returns(resultSet);
            var service = serviceMock.Object;

            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), new LiteralScalarResolver<decimal>(new LiteralScalarResolverArgs(0)));

            var rowCount = new RowCountConstraint(differed);

            //Method under test
            rowCount.Matches(service);

            //Test conclusion            
            serviceMock.Verify(s => s.Execute(), Times.Once());
        }

    }
}
