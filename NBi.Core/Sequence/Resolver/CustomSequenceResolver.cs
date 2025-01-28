using Microsoft.CSharp;
using NBi.Core.Assemblies;
using NBi.Core.Scalar.Casting;
using NBi.Extensibility.Resolving;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

class CustomSequenceResolver<T> : AbstractCustomFactory<ISequenceResolver>, ISequenceResolver<T>
{
    private CustomSequenceResolverArgs Args { get; }

    public CustomSequenceResolver(CustomSequenceResolverArgs args)
    => Args = args;

    protected override string CustomKind => "custom evaluation of a sequence";

    public List<T> Execute()
    {
        var instance = Instantiate(Args);
        var value = instance.Execute();
        return value.Cast<T>().ToList();
    }

    IList ISequenceResolver.Execute() => Execute();

    object IResolver.Execute() => Execute();
}