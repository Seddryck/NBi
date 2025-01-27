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
using Expressif.Predicates.Numeric;

namespace NBi.Core.Testing.Scalar.Resolver;

public class ContextScalarResolverTest
{
    [Test]
    public void Execute_FirstRowByName_CorrectEvaluation()
    {
        var rs = new DataTableResultSet();
        rs.Load(new[] { ["a", 1], new object[] { "b", 2 } });
        rs.GetColumn(0)!.Rename("Foo");

        var context = new Context();
        var args = new ContextScalarResolverArgs(context, new ColumnNameIdentifier("Foo"));
        var resolver = new ContextScalarResolver<string>(args);

        context.Switch(rs[0]);
        Assert.That(resolver.Execute(), Is.EqualTo("a"));
    }

    [Test]
    public void Execute_FirstRowByOrdinal_CorrectEvaluation()
    {
        var rs = new DataTableResultSet();
        rs.Load(new[] { ["a", 1], new object[] { "b", 2 } });

        var context = new Context();
        var args = new ContextScalarResolverArgs(context, new ColumnOrdinalIdentifier(0));
        var resolver = new ContextScalarResolver<string>(args);

        context.Switch(rs[0]);
        Assert.That(resolver.Execute(), Is.EqualTo("a"));
    }

    [Test]
    public void Execute_SecondRow_CorrectEvaluation()
    {
        var rs = new DataTableResultSet();
        rs.Load(new[] { ["a", 1], new object[] { "b", 2 } });
        rs.GetColumn(0)!.Rename("Foo");

        var context = new Context();
        var args = new ContextScalarResolverArgs(context, new ColumnNameIdentifier("Foo"));
        var resolver = new ContextScalarResolver<string>(args);

        context.Switch(rs[0]);
        Assert.That(resolver.Execute(), Is.EqualTo("a"));
        context.Switch(rs[1]);
        Assert.That(resolver.Execute(), Is.EqualTo("b"));
    }
}
