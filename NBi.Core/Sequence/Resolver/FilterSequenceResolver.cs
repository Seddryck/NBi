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

public class FilterSequenceResolver<T> : ISequenceResolver<T>
{
    private FilterSequenceResolverArgs Args { get; }

    public FilterSequenceResolver(FilterSequenceResolverArgs args)
        => Args = args;

    public List<T> Execute()
    {
        var candidates = Args.Resolver.Execute().Cast<T>().ToList();
        var transfomedValues = Args.OperandTransformation == null ? candidates.Cast<object>() : candidates.Select(c => Args.OperandTransformation.Execute(c!));
        var zip = candidates.Zip(transfomedValues, (x, y) => new { Original = x, Transformed = y });
        return zip.Where(x => Args.Predicate.Execute(x.Transformed ?? throw new NotSupportedException())).Select(x => x.Original).ToList();
    }

    IList ISequenceResolver.Execute() => Execute();

    object IResolver.Execute() => Execute();
}