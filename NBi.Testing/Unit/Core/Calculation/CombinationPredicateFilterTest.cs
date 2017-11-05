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

namespace NBi.Testing.Unit.Core.Calculation
{
    public class CombinationPredicateFilterTest
    {

        [Test]
        public void Apply_And_CorrectResult()
        {
            var service = new ObjectArrayResultSetResolver(
                new object[] 
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "(empty)", 20, 75 },
                    new List<object>() { "C", 5, 50 }
                });

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var info1 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Operand == "a"
                );
            var info2 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.MoreThanOrEqual
                        && p.ColumnType == ColumnType.Numeric
                        && p.Operand == "#1"
                        && p.Reference == (object)10
                );
            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.And , new[] { info1, info2 });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_Or_CorrectResult()
        {
            var service = new ObjectArrayResultSetResolver(
                new object[]
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "(empty)", 20, 75 },
                    new List<object>() { "C", 5, 50 },
                    new List<object>() { "C", 15, 50 }
                });

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var info1 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Operand == "a"
                );
            var info2 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.LessThan
                        && p.ColumnType == ColumnType.Numeric
                        && p.Operand == "#1"
                        && p.Reference == (object)10
                );
            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.Or, new[] { info1, info2 });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(4));
        }

        [Test]
        public void Apply_XOr_CorrectResult()
        {
            var service = new ObjectArrayResultSetResolver(
                new object[]
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "(empty)", 20, 75 },
                    new List<object>() { "C", 5, 50 },
                    new List<object>() { "C", 15, 50 }
                });

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var info1 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Operand == "a"
                );
            var info2 = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.LessThan
                        && p.ColumnType == ColumnType.Numeric
                        && p.Operand == "#1"
                        && p.Reference == (object)10
                );
            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], CombinationOperator.XOr, new[] { info1, info2 });
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(3));
        }

    }
}
