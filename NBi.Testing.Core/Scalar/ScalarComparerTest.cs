using NBi.Core.Scalar;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar;

public class ScalarComparerTest
{
    [TestCase(1,2)]
    [TestCase(-1, 2)]
    [TestCase(20, 1)]
    [TestCase(10, 2)]
    public void Compare_IntToInt_CorrectResult(int x, int y)
    {
        var comparer = new ScalarComparer<int>();
        Assert.That(comparer.Compare(x, y), Is.EqualTo(x.CompareTo(y)));
    }

    [TestCase(1, 2)]
    [TestCase(-1, 2)]
    [TestCase(20, 1)]
    [TestCase(10, 2)]
    public void Compare_StringToString_CorrectResult(int x, int y)
    {
        var comparer = new ScalarComparer<string>();
        Assert.That(comparer.Compare(x.ToString(), y.ToString()), Is.EqualTo(x.ToString().CompareTo(y.ToString())));
    }

    [TestCase(1, 2)]
    [TestCase(-1, 2)]
    [TestCase(20, 1)]
    [TestCase(10, 2)]
    public void Compare_IntToResolver_CorrectResult(int x, int y)
    {
        var args = new LiteralScalarResolverArgs(y);
        var resolver = new LiteralScalarResolver<int>(args);

        var comparer = new ScalarComparer<int>();
        Assert.That(comparer.Compare(x, resolver), Is.EqualTo(x.CompareTo(y)));
    }

    [TestCase(1, 2)]
    [TestCase(-1, 2)]
    [TestCase(20, 1)]
    [TestCase(10, 2)]
    public void Compare_ResolverToInt_CorrectResult(int x, int y)
    {
        var args = new LiteralScalarResolverArgs(x);
        var resolver = new LiteralScalarResolver<int>(args);

        var comparer = new ScalarComparer<int>();
        Assert.That(comparer.Compare(resolver, y), Is.EqualTo(x.CompareTo(y)));
    }


    [TestCase(1, 2)]
    [TestCase(-1, 2)]
    [TestCase(20, 1)]
    [TestCase(10, 2)]
    public void Compare_ResolverToResolver_CorrectResult(int x, int y)
    {
        var xArgs = new LiteralScalarResolverArgs(x);
        var xResolver = new LiteralScalarResolver<int>(xArgs);

        var yArgs = new LiteralScalarResolverArgs(y);
        var yResolver = new LiteralScalarResolver<int>(yArgs);

        var comparer = new ScalarComparer<int>();
        Assert.That(comparer.Compare(xResolver, yResolver), Is.EqualTo(x.CompareTo(y)));
    }
}
