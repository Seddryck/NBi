using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class ListSequenceResolver<T> : ISequenceResolver<T>
{
    private readonly ListSequenceResolverArgs args;

    public ListSequenceResolver(ListSequenceResolverArgs args)
    {
        this.args = args;
    }

    public ListSequenceResolver(IEnumerable<T> values)
    {
        var resolvers = new List<IScalarResolver<T>>();
        foreach (var value in values)
            resolvers.Add(new LiteralScalarResolver<T>(value ?? throw new NotSupportedException()));

        args = new ListSequenceResolverArgs(resolvers);
    }

    IList ISequenceResolver.Execute() => this.Execute();
    object IResolver.Execute() => this.Execute();

    public List<T> Execute()
    {
        var list = new List<T>();
        foreach (var resolver in args.Resolvers)
        {
            var obj = resolver.Execute();
            var caster = new CasterFactory<T>().Instantiate();
            var value = caster.Execute(obj);
            list.Add(value);
        }
            
        return list;
    }
}
