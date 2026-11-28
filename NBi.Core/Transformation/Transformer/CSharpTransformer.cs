using NBi.Core.Assemblies;
using NBi.Core.Injection;
using NBi.Core.Scalar.Casting;
using NBi.Core.Variable;

namespace NBi.Core.Transformation.Transformer;

class CSharpTransformer<T> : ITransformer
{
    protected Context Context { get; }
    private Func<T, object?>? evaluate;

    public CSharpTransformer()
        : this(null, Context.None) { }
    public CSharpTransformer(ServiceLocator? serviceLocator, Context context)
        => (Context) = (context);

    public void Initialize(string code)
        => evaluate = CSharpRuntime.Instance.CompileTransformation<T>(code);

    public object? Execute(object value)
    {
        if (evaluate is null)
            throw new InvalidOperationException();

        var factory = new CasterFactory<T>();
        var caster = factory.Instantiate();
        var typedValue = caster.Execute(value);
        var transformedValue = evaluate(typedValue);

        return transformedValue;
    }
}
