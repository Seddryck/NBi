using Moq;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Testing.Core.Scalar.Resolver
{
    public class ContextScalarResolverTest
    {
        [Test]
        public void Execute_FirstRowByName_CorrectEvaluation()
        {
            var rs = new NBi.Core.ResultSet.DataTableResultSet();
            rs.Load(new[] { new object[] { "a", 1 }, new object[] { "b", 2 } });
            rs.Columns[0].ColumnName = "Foo";

            var context = Context.None;
            var args = new ContextScalarResolverArgs(context, new ColumnNameIdentifier("Foo"));
            var resolver = new ContextScalarResolver<string>(args);

            context.Switch(rs.Rows[0]);
            Assert.That(resolver.Execute(), Is.EqualTo("a"));
        }

        [Test]
        public void Execute_FirstRowByOrdinal_CorrectEvaluation()
        {
            var rs = new NBi.Core.ResultSet.DataTableResultSet();
            rs.Load(new[] { new object[] { "a", 1 }, new object[] { "b", 2 } });

            var context = Context.None;
            var args = new ContextScalarResolverArgs(context, new ColumnOrdinalIdentifier(0));
            var resolver = new ContextScalarResolver<string>(args);

            context.Switch(rs.Rows[0]);
            Assert.That(resolver.Execute(), Is.EqualTo("a"));
        }

        [Test]
        public void Execute_SecondRow_CorrectEvaluation()
        {
            var rs = new NBi.Core.ResultSet.DataTableResultSet();
            rs.Load(new[] { new object[] { "a", 1 }, new object[] { "b", 2 } });
            rs.Columns[0].ColumnName = "Foo";

            var context = Context.None;
            var args = new ContextScalarResolverArgs(context, new ColumnNameIdentifier("Foo"));
            var resolver = new ContextScalarResolver<string>(args);

            context.Switch(rs.Rows[0]);
            Assert.That(resolver.Execute(), Is.EqualTo("a"));
            context.Switch(rs.Rows[1]);
            Assert.That(resolver.Execute(), Is.EqualTo("b"));
        }
    }
}
