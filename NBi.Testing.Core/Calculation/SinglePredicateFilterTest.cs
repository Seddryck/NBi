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
using NBi.Core.Transformation;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Injection;
using NBi.Core.Variable;

namespace NBi.Testing.Core.Calculation
{
    public class SinglePredicateFilterTest
    {

        [Test]
        public void Apply_Variable_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var predicate = new Mock<PredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("a"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_ColumnOrdinal_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));
            var rs = service.Execute();

            var predicate = new Mock<PredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);
            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnOrdinalIdentifier(0));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
            Assert.That(filter.Describe(), Does.Contain("null").And.Contain("or empty"));
        }

        [Test]
        public void Apply_ColumnName_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));
            var rs = service.Execute();
            rs.Table.Columns[0].ColumnName = "first";

            var predicate = new Mock<PredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);
            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("first"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_ColumnNameCaseNotMatching_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));
            var rs = service.Execute();
            rs.Table.Columns[0].ColumnName = "first";

            var predicate = new Mock<PredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("FirSt"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }


        [Test]
        public void Apply_UnexistingColumnName_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { "(null)", 10, 100 },
                        new List<object>() { "(empty)", 2, 75 },
                        new List<object>() { "C", 5, 50 }
                    }));
            var rs = service.Execute();
            rs.Table.Columns[0].ColumnName = "first";
            rs.Table.Columns[1].ColumnName = "second";
            rs.Table.Columns[2].ColumnName = "third";

            var predicate = new Mock<ReferencePredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.NullOrEmpty);
            predicate.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(200));

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("Unexisting"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], predication.Object);
            var ex = Assert.Throws<ArgumentException>(() => filter.Apply(rs));
            Assert.That(ex.Message, Does.Contain("first"));
            Assert.That(ex.Message, Does.Contain("second"));
            Assert.That(ex.Message, Does.Contain("third"));
        }

        [Test]
        public void Apply_NestedExpression_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                        {
                            new List<object>() { 1, 10, 100 },
                            new List<object>() { 2, 2, 75 },
                            new List<object>() { 3, 5, 50 }
                        }));
            var rs = service.Execute();

            var aliases = new List<IColumnAlias>()
            {
                Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a"),
                Mock.Of<IColumnAlias>(v => v.Column == 1 && v.Name == "b"),
                Mock.Of<IColumnAlias>(v => v.Column == 2 && v.Name == "c")
            };

            var expressions = new List<IColumnExpression>()
            {
                Mock.Of<IColumnExpression>(e => e.Value == "Abs([a])+[e]" && e.Name == "d" && e.Language == LanguageType.NCalc),
                Mock.Of<IColumnExpression>(e => e.Value == "[b]*[c]" && e.Name == "e" && e.Language == LanguageType.NCalc)
            };

            var predicate = new Mock<ReferencePredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.MoreThanOrEqual);
            predicate.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(200));

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("d"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(aliases, expressions, predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_MixedExpression_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { 1, 10, 100 },
                        new List<object>() { 2, 2, 75 },
                        new List<object>() { 3, 5, 50 }
                    }));
            var rs = service.Execute();
            rs.Table.Columns[2].ColumnName = "c1";

            var aliases = new List<IColumnAlias>()
            {
                Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a"),
            };

            var expressions = new List<IColumnExpression>()
            {
                Mock.Of<IColumnExpression>(e => e.Value == "Abs([a])+[e]" && e.Name == "d" && e.Language == LanguageType.NCalc),
                Mock.Of<IColumnExpression>(e => e.Value == "[#1]*[c1]" && e.Name == "e" && e.Language == LanguageType.NCalc)
            };

            var predicate = new Mock<ReferencePredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.MoreThanOrEqual);
            predicate.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<decimal>(200));

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("d"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);

            var factory = new ResultSetFilterFactory(null, new Context(null));
            var filter = factory.Instantiate(aliases, expressions, predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_NativeExpression_CorrectResult()
        {
            var service = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new object[]
                    {
                        new List<object>() { new DateTime(2019, 10, 01, 8, 0, 0), 10, 100 },
                        new List<object>() { new DateTime(2019, 10, 01, 23, 0, 0), 2, 75 },
                        new List<object>() { new DateTime(2019, 10, 02, 05, 0, 0), 5, 50 }
                    }));
            var rs = service.Execute();
            rs.Table.Columns[0].ColumnName = "a";

            var aliases = new List<IColumnAlias>()
            {
                Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "x"),
            };

            var expressions = new List<IColumnExpression>()
            {
                Mock.Of<IColumnExpression>(
                    e => e.Value == "a | utc-to-local(Brussels) | dateTime-to-date" 
                    && e.Name == "d" 
                    && e.Language == LanguageType.Native 
                    && e.Type==ColumnType.DateTime),
            };

            var predicate = new Mock<ReferencePredicateArgs>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.DateTime);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.MoreThanOrEqual);
            predicate.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<DateTime>(new DateTime(2019, 10, 2)));

            var predication = new Mock<PredicationArgs>();
            predication.SetupGet(p => p.Identifier).Returns(new ColumnNameIdentifier("d"));
            predication.SetupGet(p => p.Predicate).Returns(predicate.Object);


            var factory = new ResultSetFilterFactory(new ServiceLocator(), new Context(null));
            var filter = factory.Instantiate(aliases, expressions, predication.Object);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

    }
}
