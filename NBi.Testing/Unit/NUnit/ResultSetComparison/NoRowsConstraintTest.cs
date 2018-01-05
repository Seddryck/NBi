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
using NBi.Core.ResultSet.Resolver;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class NoRowsConstraintTest
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

            var serviceMock = new Mock<IResultSetService>();
            serviceMock.Setup(s => s.Execute())
                .Returns(resultSet);
            var service = serviceMock.Object;

            var alias = Mock.Of<IColumnAlias>(v => v.Column == 2 && v.Name == "Value");
            
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.Equal);
            predicate.SetupGet(p => p.Operand).Returns("Value");
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)1);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>() { alias }
                    , new List<IColumnExpression>() { }
                    , predicate.Object
                );

            var rowCount = new NoRowsConstraint(filter);

            //Method under test
            rowCount.Matches(service);

            //Test conclusion            
            serviceMock.Verify(s => s.Execute(), Times.Once());
        }

        [Test]
        public void Matches_AllValidatePredicate_False()
        {
            var rs = new ResultSet();
            rs.Load(new[] { new object[] { "a", -1 }, new object[] { "b", -2 }, new object[] { "c", -3 } });

            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate.SetupGet(p => p.Operand).Returns("#1");
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)0);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>()
                    , new List<IColumnExpression>()
                    , predicate.Object
                );

            var singleRowCtr = new NoRowsConstraint(filter);
            Assert.That(singleRowCtr.Matches(rs), Is.False);
        }

        [Test]
        public void Matches_NoneValidatePredicate_True()
        {
            var rs = new ResultSet();
            rs.Load(new[] { new object[] { "a", 1 }, new object[] { "b", 2 }, new object[] { "c", 3 } });

            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate.SetupGet(p => p.Operand).Returns("#1");
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)0);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>()
                    , new List<IColumnExpression>()
                    , predicate.Object
                );

            var singleRowCtr = new NoRowsConstraint(filter);
            Assert.That(singleRowCtr.Matches(rs), Is.True);
        }

        [Test]
        public void Matches_FewValidatePredicate_False()
        {
            var rs = new ResultSet();
            rs.Load(new[] { new object[] { "a", -1 }, new object[] { "b", -2 }, new object[] { "c", 3 } });

            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate.SetupGet(p => p.Operand).Returns("#1");
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)0);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>()
                    , new List<IColumnExpression>()
                    , predicate.Object
                );

            var singleRowCtr = new NoRowsConstraint(filter);
            Assert.That(singleRowCtr.Matches(rs), Is.False);
        }

        [Test]
        public void Matches_SingleValidatePredicate_False()
        {
            var rs = new ResultSet();
            rs.Load(new[] { new object[] { "a", -1 }, new object[] { "b", 2 }, new object[] { "c", 3 } });

            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate.SetupGet(p => p.Operand).Returns("#1");
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)0);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                (
                    new List<IColumnAlias>()
                    , new List<IColumnExpression>()
                    , predicate.Object
                );

            var singleRowCtr = new NoRowsConstraint(filter);
            Assert.That(singleRowCtr.Matches(rs), Is.False);
        }
    }
}
