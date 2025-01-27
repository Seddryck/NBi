using NBi.Core.Scalar;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

public abstract class RuntimeVariable : IRuntimeVariable
{
    private object? value;
    private bool isEvaluated = false;
    private IScalarResolver Resolver { get; }

    public RuntimeVariable(IScalarResolver resolver)
        => Resolver = resolver;

    public void Evaluate()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        value = Resolver.Execute();
        isEvaluated = true;

        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Time needed for evaluation of the variable: {stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}");

        var invariantCulture = new CultureFactory().Invariant;
        var msg = $"Variable evaluated to: {value}";
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, msg.ToString(invariantCulture));
    }

    public virtual object? GetValue()
    {
        if (!IsEvaluated())
            Evaluate();
        return value;
    }

    public virtual bool IsEvaluated() => isEvaluated;
}
