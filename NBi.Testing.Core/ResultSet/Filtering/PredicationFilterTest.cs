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
using NBi.Core.Calculation.Asserting;
using NBi.Core.Variable;
using NBi.Core.ResultSet.Filtering;
using Expressif.Values;

namespace NBi.Core.Testing.ResultSet.Filtering
{
    [Ignore("Not supported by Expressif")]
    public class PredicationFilterTest
    {

        [Test]
        public void Apply_And_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    [
                        new object[] { "(null)", 10, 100 },
                        new object[] { "(empty)", 2, 75 },
                        new object[] { "(empty)", 20, 75 },
                        new object[] { "C", 5, 50 }
                    ]));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new PredicateArgs()
            {
                ColumnType = ColumnType.Text,
                ComparerType = ComparerType.NullOrEmpty
            };
            
            var predication1 = new PredicationArgs(predicate1, new ColumnNameIdentifier("a"));

            var predicate2 = new ReferencePredicateArgs(new LiteralScalarResolver<decimal>(10))
            {
                ColumnType = ColumnType.Numeric,
                ComparerType = ComparerType.MoreThanOrEqual
            };
            var predication2 = new PredicationArgs(predicate2, new ColumnOrdinalIdentifier(1));


            var factory = new ResultSetFilterFactory(new());
            var filter = factory.Instantiate(CombinationOperator.And, [predication1, predication2], new(new(), aliases, []));
            var result = filter.Apply(rs);

            Assert.That(result.Rows.Count(), Is.EqualTo(2));
        }


        [Test]
        public void Apply_AndWillNotEvaluateAll_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object?[]
                    {
                        new object?[] { null },
                        new object?[] { 5 },
                        new object?[] { 10 },
                        new object?[] { null },
                        new object?[] { 20 },
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate1 = new PredicateArgs()
            {
                ColumnType = ColumnType.Numeric,
                ComparerType = ComparerType.Null,
                Not = true
            };
            var predication1 = new PredicationArgs(predicate1, new ColumnOrdinalIdentifier(0));

            var predicate2 = new ReferencePredicateArgs(new LiteralScalarResolver<decimal>(10))
            {
                ColumnType = ColumnType.Numeric,
                ComparerType = ComparerType.LessThan,
                Not = true
            };
            var predication2 = new PredicationArgs(predicate2, new ColumnOrdinalIdentifier(0));

            var factory = new ResultSetFilterFactory(new Core.Injection.ServiceLocator());
            var filter = factory.Instantiate(CombinationOperator.And, [predication1, predication2], new Context(new ContextVariables(), aliases, Array.Empty<IColumnExpression>()));
            var result = filter.Apply(rs);

            Assert.That(result.Rows.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Apply_Or_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new object[] { "(null)", 10, 100 },
                        new object[] { "(empty)", 2, 75 },
                        new object[] { "(empty)", 20, 75 },
                        new object[] { "C", 5, 50 },
                        new object[] { "C", 15, 50 }
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


            var factory = new ResultSetFilterFactory(new Core.Injection.ServiceLocator());
            var filter = factory.Instantiate(CombinationOperator.Or, [predication1.Object, predication2.Object], new Context(new ContextVariables(), aliases, Array.Empty<IColumnExpression>()));
            var result = filter.Apply(rs);

            Assert.That(result.Rows.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Apply_OrWillNotEvaluateAll_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    [
                        new object?[] { null },
                        new object?[] { 5 },
                        new object?[] { 10 },
                        new object?[] { null },
                        new object?[] { 20 },
                    ]));

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

            var factory = new ResultSetFilterFactory(new Core.Injection.ServiceLocator());
            var filter = factory.Instantiate(CombinationOperator.Or, [predication1.Object, predication2.Object], new Context(new ContextVariables(), aliases, Array.Empty<IColumnExpression>()));
            var result = filter.Apply(rs);

            Assert.That(result.Rows.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Apply_XOr_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new object[] { "(null)", 10, 100 },
                        new object[] { "(empty)", 2, 75 },
                        new object[] { "(empty)", 20, 75 },
                        new object[] { "C", 5, 50 },
                        new object[] { "C", 15, 50 }
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

            var factory = new ResultSetFilterFactory(new Core.Injection.ServiceLocator());
            var filter = factory.Instantiate(CombinationOperator.XOr, [predication1.Object, predication2.Object], new Context(new ContextVariables(), aliases, Array.Empty<IColumnExpression>()));
            var result = filter.Apply(rs);

            Assert.That(result.Rows.Count(), Is.EqualTo(3));
        }

    }
}
