using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core;
using NBi.Core.Calculation;
using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Calculation.Predicate;

namespace NBi.Testing.Core.Calculation
{
    public class CombinationPredicateFilterTest
    {

        [Test]
        public void Apply_And_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "(empty)", 20, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new Mock<PredicateArgs>();
            predicate1.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate1.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);

            var predication1 = new Mock<PredicationArgs>();
            predication1.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("a"));
            predication1.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var predicate2 = new Mock<ReferencePredicateArgs>();
            predicate2.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate2.SetupGet(p => p.ComparerType).Returns(ComparerType.MoreThanOrEqual);
            predicate2.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(10));

            var predication2 = new Mock<PredicationArgs>();
            predication2.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(1));
            predication2.SetupGet(p => p.Predicate).Returns(predicate2.Object);
            

            var factory = new ResultSetFilterFactory(null, null);
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.And , new[] { predication1.Object, predication2.Object });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }


        [Test]
        public void Apply_AndWillNotEvaluateAll_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { null },
                        new List<object>() { 5 },
                        new List<object>() { 10 },
                        new List<object>() { null },
                        new List<object>() { 20 },
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new Mock<PredicateArgs>();
            predicate1.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate1.SetupGet(p => p.ComparerType).Returns(ComparerType.Null);
            predicate1.SetupGet(p => p.Not).Returns(true);
            var predication1 = new Mock<PredicationArgs>();
            predication1.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(0));
            predication1.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var predicate2 = new Mock<ReferencePredicateArgs>();
            predicate2.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate2.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate2.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(10));
            var predication2 = new Mock<PredicationArgs>();
            predication2.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(0));
            predication2.SetupGet(p => p.Predicate).Returns(predicate2.Object);

            var factory = new ResultSetFilterFactory(null, null);
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.And, new[] { predication1.Object, predication2.Object });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(1));
        }

        [Test]
        public void Apply_Or_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "(empty)", 20, 75 },
                        new List<object>() { "C", 5, 50 },
                        new List<object>() { "C", 15, 50 }
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new Mock<PredicateArgs>();
            predicate1.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate1.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);
            var predication1 = new Mock<PredicationArgs>();
            predication1.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("a"));
            predication1.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var predicate2 = new Mock<ReferencePredicateArgs>();
            predicate2.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate2.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate2.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(10));
            var predication2 = new Mock<PredicationArgs>();
            predication2.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(1));
            predication2.SetupGet(p => p.Predicate).Returns(predicate2.Object);
            

            var factory = new ResultSetFilterFactory(null, null);
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.Or, new[] { predication1.Object, predication2.Object });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(4));
        }

        [Test]
        public void Apply_OrWillNotEvaluateAll_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { null },
                        new List<object>() { 5 },
                        new List<object>() { 10 },
                        new List<object>() { null },
                        new List<object>() { 20 },
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new Mock<PredicateArgs>();
            predicate1.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate1.SetupGet(p => p.ComparerType).Returns(ComparerType.Null);
            var predication1 = new Mock<PredicationArgs>();
            predication1.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(0));
            predication1.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var predicate2 = new Mock<ReferencePredicateArgs>();
            predicate2.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate2.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate2.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(10));
            var predication2 = new Mock<PredicationArgs>();
            predication2.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(0));
            predication2.SetupGet(p => p.Predicate).Returns(predicate2.Object);

            var factory = new ResultSetFilterFactory(null, null);
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.Or, new[] { predication1.Object, predication2.Object });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(3));
        }

        [Test]
        public void Apply_XOr_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "(empty)", 20, 75 },
                        new List<object>() { "C", 5, 50 },
                        new List<object>() { "C", 15, 50 }
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new Mock<PredicateArgs>();
            predicate1.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate1.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);
            var predication1 = new Mock<PredicationArgs>();
            predication1.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("a"));
            predication1.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var predicate2 = new Mock<ReferencePredicateArgs>();
            predicate2.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate2.SetupGet(p => p.ComparerType).Returns(ComparerType.LessThan);
            predicate2.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(10));
            var predication2 = new Mock<PredicationArgs>();
            predication2.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(1));
            predication2.SetupGet(p => p.Predicate).Returns(predicate1.Object);

            var factory = new ResultSetFilterFactory(null, null);
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.XOr, new[] { predication1.Object, predication2.Object });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(3));
        }

    }
}
