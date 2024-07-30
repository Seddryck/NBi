using NBi.Core.ResultSet.Combination;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using Moq;

namespace NBi.Core.Testing.ResultSet.Resolver
{
    public class EmptyResultSetResolverTest
    {
        [Test()]
        public void Instantiate_ColumnsBased_CorrectType()
        {
            var args = new EmptyResultSetResolverArgs(
                [
                    new ColumnNameIdentifier("myFirstColumn"),
                    new ColumnNameIdentifier("mySecondColumn"),
                ]
            );
            var resolver = new EmptyResultSetResolver(args);

            var rs = resolver.Execute();
            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs?.GetColumn(0)?.Name, Is.EqualTo("myFirstColumn"));
            Assert.That(rs?.GetColumn(1)?.Name, Is.EqualTo("mySecondColumn"));
        }

        [Test()]
        public void Instantiate_ColumnCountBased_CorrectType()
        {
            var args = new EmptyResultSetResolverArgs(new LiteralScalarResolver<int>(4));
            var resolver = new EmptyResultSetResolver(args);

            var rs = resolver.Execute();
            Assert.That(rs.ColumnCount, Is.EqualTo(4));
        }

        [Test()]
        public void Instantiate_ColumnsAndColumnCountBased_CorrectType()
        {
            var args = new EmptyResultSetResolverArgs(
                [
                    new ColumnNameIdentifier("myFirstColumn"),
                    new ColumnNameIdentifier("mySecondColumn"),
                ], new LiteralScalarResolver<int>(4)
            );
            var resolver = new EmptyResultSetResolver(args);

            var rs = resolver.Execute();
            Assert.That(rs.ColumnCount, Is.EqualTo(4));
            Assert.That(rs?.GetColumn(0)?.Name, Is.EqualTo("myFirstColumn"));
            Assert.That(rs?.GetColumn(1)?.Name, Is.EqualTo("mySecondColumn"));
        }
    }
}