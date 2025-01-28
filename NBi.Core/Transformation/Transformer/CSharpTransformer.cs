using Microsoft.CSharp;
using NBi.Core.Compiling;
using NBi.Core.Injection;
using NBi.Core.Scalar.Casting;
using NBi.Core.Variable;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer;

class CSharpTransformer<T> : ITransformer
{
    protected Context Context { get; }
    private TransformerCompiler<T>? Compiler { get; set; }

    public CSharpTransformer() : this(null, Context.None) { }
    public CSharpTransformer(ServiceLocator? serviceLocator, Context context)
        => (Context) = (context);

    public void Initialize(string code)
    {
        Compiler = new();
        Compiler.Compile(code);
    }

    public object? Execute(object value)
    {
        if (Compiler is null)
            throw new InvalidOperationException();

        var factory = new CasterFactory<T>();
        var caster = factory.Instantiate();
        var typedValue = caster.Execute(value);
        var transformedValue = Compiler.Evaluate(typedValue);

        return transformedValue;
    }
}
