using NBi.Core.Injection;
using NBi.Core.Variable;
using NCalc;

namespace NBi.Core.Transformation.Transformer;

class NCalcTransformer<T> : ITransformer
{
    protected Context Context { get; }
    private Expression? method;

    public NCalcTransformer() 
        : this(null, Context.None) { }
    public NCalcTransformer(ServiceLocator? serviceLocator, Context context)
        => (Context) = (context);

    public void Initialize(string code)
    {
       method = new Expression(code);
    }

    public object Execute(object value)
    {
        if (method is null)
            throw new InvalidOperationException();

        if (method.Parameters.ContainsKey("value"))
            method.Parameters["value"] = value;
        else
            method.Parameters.Add("value", value);

        var transformedValue = method.Evaluate();

        return transformedValue;
    }
}
