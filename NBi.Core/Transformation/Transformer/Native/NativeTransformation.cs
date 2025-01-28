using Expressif.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native;

internal class NativeTransformation(Expressif.Expression expression) : INativeTransformation
{
    protected IFunction InternalFunction { get; } = expression;

    public object? Evaluate(object? value)
        => InternalFunction.Evaluate(value);
}
