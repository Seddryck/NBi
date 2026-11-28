using NBi.Core.Assemblies;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Scalar.Resolver;

class CSharpScalarResolver<T> : IScalarResolver<T>
{
    private CSharpScalarResolverArgs Args { get; }
    private Func<object?>? evaluate;

    public CSharpScalarResolver(CSharpScalarResolverArgs args)
        => (Args) = (args);

    public CSharpScalarResolver(string code)
        : this(new CSharpScalarResolverArgs(code))
    { }

    public T? Execute()
    {
        evaluate ??= CSharpRuntime.Instance.CompileScalar(Args.Code);
        var value = evaluate();
        return (T?)Convert.ChangeType(value, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}
