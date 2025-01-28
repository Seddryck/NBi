using Microsoft.CSharp;
using NBi.Core.Assemblies;
using NBi.Extensibility.Resolving;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class CustomScalarResolver<T> : AbstractCustomFactory<IScalarResolver>, IScalarResolver<T>
{
    private CustomScalarResolverArgs Args {get;}

    public CustomScalarResolver(CustomScalarResolverArgs args)
    => Args = args;

    protected override string CustomKind => "custom evaluation of a scalar";

    public T? Execute()
    {
        var instance = Instantiate(Args);
        var value = instance.Execute();
        return (T?)Convert.ChangeType(value, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}