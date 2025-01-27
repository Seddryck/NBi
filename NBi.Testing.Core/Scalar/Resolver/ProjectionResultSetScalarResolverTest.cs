using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver;

public class ProjectionResultSetScalarResolverTest
{
    [Test]
    public void Execute_RowCount_CorrectResult()
    {
        var rsArgs = new ObjectsResultSetResolverArgs(
            new List<object[]>()
            {
                new object[] { "alpha", 1 },
                new object[] { "beta", 2 },
                new object[] { "Gamma", 3 }
            });
        var args = new RowCountResultSetScalarResolverArgs(rsArgs);
        var resolver = new ProjectionResultSetScalarResolver<int>(args, new ResultSetResolverFactory(new Core.Injection.ServiceLocator()));
        Assert.That(resolver.Execute(), Is.EqualTo(3));
    }

    [Test]
    public void Execute_RowCountEmptyResultSet_CorrectResult()
    {
        var rsArgs = new ObjectsResultSetResolverArgs(
            new List<object[]>()
            {
                
            });
        var args = new RowCountResultSetScalarResolverArgs(rsArgs);
        var resolver = new ProjectionResultSetScalarResolver<int>(args, new ResultSetResolverFactory(new Core.Injection.ServiceLocator()));
        Assert.That(resolver.Execute(), Is.EqualTo(0));
    }
}
