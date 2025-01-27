using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class ContextScalarResolver<T> : IScalarResolver<T>
{
    private Context Context { get; }
    private IColumnIdentifier ColumnIdentifier { get; }

    public ContextScalarResolver(ContextScalarResolverArgs args)
        => (Context, ColumnIdentifier) = (args.Context, args.ColumnIdentifier);

    internal ContextScalarResolver(Context context, IColumnIdentifier columnIdentifier)
        => (Context, ColumnIdentifier) = (context, columnIdentifier);

    public T? Execute()
    {
        var evaluation = Context.CurrentRow?[ColumnIdentifier];
        if (evaluation == null)
            return default;
        var typedEvaluation = StrongTypingVariable(evaluation);
        return typedEvaluation is null ? default : (T)typedEvaluation;
    }

    object? IResolver.Execute() => Execute();

    private static object? StrongTypingVariable(object input)
    {
        IFormatProvider formatProvider = typeof(T) == typeof(DateTime)
            ? System.Globalization.DateTimeFormatInfo.InvariantInfo
            : System.Globalization.NumberFormatInfo.InvariantInfo;
        var toString = input.ToString() ?? string.Empty;

        if (input != null && toString.EndsWith("%"))
            input = toString[..^1];

        var output = Convert.ChangeType(input, typeof(T), formatProvider);
        return output;
    }
}
