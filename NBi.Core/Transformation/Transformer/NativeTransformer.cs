using Microsoft.CSharp;
using NBi.Core.Injection;
using NBi.Core.Scalar.Casting;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using Exssif = Expressif.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer;

class NativeTransformer<T> : ITransformer
{
    protected ServiceLocator ServiceLocator { get; }
    protected Context Context { get; }

    protected Exssif.IFunction? InternalTransformation { get; set; }


    public NativeTransformer(ServiceLocator serviceLocator, Context context)
        => (ServiceLocator, Context) = (serviceLocator, context);

    public void Initialize(string code)
    {
        InternalTransformation = new Expressif.Expression(code, Context);
    }

    public object? Execute(object? value)
    {
        if (InternalTransformation is null)
            throw new InvalidOperationException();

        var factory = new CasterFactory<T>();
        var caster = factory.Instantiate();

        object? typedValue;
        if (value == null || value == DBNull.Value || value as string == "(null)")
            typedValue = null;
        else if ((typeof(T) != typeof(string)) && (value is string) && ((string.IsNullOrEmpty(value as string) || value as string == "(empty)")))
            typedValue = null;
        else
            typedValue = caster.Execute(value);

        return InternalTransformation.Evaluate(typedValue);
    }
}
