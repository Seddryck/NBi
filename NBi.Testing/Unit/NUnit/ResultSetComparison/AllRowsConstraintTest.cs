using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Query;
using NBi.Core.ResultSet;
using System.Data.SqlClient;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;
using System.Collections.Generic;
using NUnit.Framework;
using NBi.Core.ResultSet.Loading;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class AllRowsConstraintTest
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
        public void Matches_ResultSetService_CallToExecuteOnce()
        {
            var resultSet = new ResultSet();
            resultSet.Load("a;b;1");

            var serviceMock = new Mock<IResultSetLoader>();
            serviceMock.Setup(s => s.Execute())
                .Returns(resultSet);
            var service = serviceMock.Object;

            var alias = Mock.Of<IColumnAlias>(v => v.Column == 2 && v.Name == "Value");
            var predicate = Mock.Of<IPredicateInfo>
                (
                    p => p.ColumnType==ColumnType.Numeric 
                    && p.ComparerType==ComparerType.Equal 
                    && p.Name=="Value" 
                    && p.Reference==(object)1
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>() { alias }
                    , new List<IColumnExpression>() { }
                    , predicate
                );

            var rowCount = new AllRowsConstraint(filter);

            //Method under test
            rowCount.Matches(service);

            //Test conclusion            
            serviceMock.Verify(s => s.Execute(), Times.Once());
        }

    }
}
