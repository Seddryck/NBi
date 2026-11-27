using Expressif;
using Expressif.Syntax;
using Context = NBi.Core.Variable.Context;
using System.Text.RegularExpressions;

namespace NBi.Core.Transformation.Transformer.Native;

internal static partial class NativeExpression
{
    // Match quoted strings and column names first so they are kept intact.
    [GeneratedRegex("""
        "(?:\\.|[^"\\])*" | '(?:\\.|[^'\\])*' | \[[^\]\r\n]*\]
        | (?<=[(,])\s*
          (?:
              (?<temporal>
                  [0-9]{4}-[0-9]{2}-[0-9]{2}(?:[T ][0-9]{2}:[0-9]{2}:[0-9]{2}(?:\.[0-9]+)?)?
                  | [0-9]{2}:[0-9]{2}:[0-9]{2}(?:\.[0-9]+)?
              )
            | (?<text> [;*] | [a-zA-Z0-9.*-]*\*[a-zA-Z0-9.*-]* | [a-zA-Z][a-zA-Z0-9]*_[a-zA-Z0-9_]+ )
          )
          \s*(?=[,)])
        """, RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex LegacyArgument();

    public static IExpression Create(string code, Context context)
    {
        try
        {
            return Expression.Create(code, context);
        }
        catch (ExpressifSyntaxException)
        {
            // Expressif 1.x accepted bare temporal and text arguments in existing NBi scripts.
            var updated = LegacyArgument().Replace(code, match =>
            {
                if (match.Groups["temporal"] is { Success: true } temporal)
                    return $"#\"{temporal.Value.Replace(' ', 'T')}\"";
                if (match.Groups["text"] is { Success: true } text)
                    return $"\"{text.Value}\"";

                return match.Value;
            });
            if (updated == code)
                throw;

            return Expression.Create(updated, context);
        }
    }
}
