using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver.Resources;

public class MyCustomClassWithParams : IScalarResolver
{
    private int Foo { get; }
    private DateTime Bar { get; }

    public MyCustomClassWithParams(DateTime bar, int foo)
        => (Bar, Foo) = (bar, foo);

    public object Execute() => Bar.AddDays(Foo);
}
