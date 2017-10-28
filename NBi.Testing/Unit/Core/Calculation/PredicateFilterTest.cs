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
using NBi.Core.ResultSet.Service;

namespace NBi.Testing.Unit.Core.Calculation
{
    public class PredicateFilterTest
    {

        [Test]
        public void Apply_Variable_CorrectResult()
        {
            var service = new ObjectArrayResultSetService(
                new object[] 
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "C", 5, 50 }
                });

            var rs = service.Execute();

            var aliases = new[] { Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a") };

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Name == "a"
                );
            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, new IColumnExpression[0], info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_ColumnIndex_CorrectResult()
        {
            var service = new ObjectArrayResultSetService(
                new object[]
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "C", 5, 50 }
                });
            var rs = service.Execute();

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Name == "#0"
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
            Assert.That(filter.Describe(), Is.StringContaining("null").And.StringContaining("or empty"));
        }

        [Test]
        public void Apply_ColumnName_CorrectResult()
        {
            var service = new ObjectArrayResultSetService(
                new object[]
                {
                    new List<object>() { "(null)", 10, 100 },
                    new List<object>() { "(empty)", 2, 75 },
                    new List<object>() { "C", 5, 50 }
                });
            var rs = service.Execute();
            rs.Table.Columns[0].ColumnName = "first";

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.NullOrEmpty
                        && p.ColumnType == ColumnType.Text
                        && p.Name == "first"
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(new IColumnAlias[0], new IColumnExpression[0], info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_NestedExpression_CorrectResult()
        {
            var service = new ObjectArrayResultSetService(
                new object[]
                {
                    new List<object>() { 1, 10, 100 },
                    new List<object>() { 2, 2, 75 },
                    new List<object>() { 3, 5, 50 }
                });
            var rs = service.Execute();

            var aliases = new List<IColumnAlias>()
            {
                Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a"),
                Mock.Of<IColumnAlias>(v => v.Column == 1 && v.Name == "b"),
                Mock.Of<IColumnAlias>(v => v.Column == 2 && v.Name == "c")
            };

            var expressions = new List<IColumnExpression>()
            {
                Mock.Of<IColumnExpression>(e => e.Value == "Abs([a])+[e]" && e.Name == "d"),
                Mock.Of<IColumnExpression>(e => e.Value == "[b]*[c]" && e.Name == "e")
            };

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType==ComparerType.MoreThanOrEqual
                        && p.ColumnType==ColumnType.Numeric
                        && p.Name == "d"
                        && p.Reference == (object)200
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, expressions, info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void Apply_MixedExpression_CorrectResult()
        {
            var service = new ObjectArrayResultSetService(
                 new object[]
                 {
                    new List<object>() { 1, 10, 100 },
                    new List<object>() { 2, 2, 75 },
                    new List<object>() { 3, 5, 50 }
                });
            var rs = service.Execute();
            rs.Table.Columns[2].ColumnName = "c1";

            var aliases = new List<IColumnAlias>()
            {
                Mock.Of<IColumnAlias>(v => v.Column == 0 && v.Name == "a"),
            };

            var expressions = new List<IColumnExpression>()
            {
                Mock.Of<IColumnExpression>(e => e.Value == "Abs([a])+[e]" && e.Name == "d"),
                Mock.Of<IColumnExpression>(e => e.Value == "[#1]*[c1]" && e.Name == "e")
            };

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType == ComparerType.MoreThanOrEqual
                        && p.ColumnType == ColumnType.Numeric
                        && p.Name == "d"
                        && p.Reference == (object)200
                );

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate(aliases, expressions, info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }

    }
}
