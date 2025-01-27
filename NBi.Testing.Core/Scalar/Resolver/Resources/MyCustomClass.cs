using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver.Resources;

public class MyCustomClass : IScalarResolver
{
    public object Execute() => "myValue";
}
