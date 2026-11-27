using Expressif;
using Expressif.Syntax;
using System.Globalization;
using System.Text.RegularExpressions;
using Context = NBi.Core.Variable.Context;

namespace NBi.Core.Transformation.Transformer.Native;

internal static partial class NativeExpression
{
    // Skip quoted strings and comments when rewriting legacy arguments and column references.
    [GeneratedRegex("""
        "(?:\\.|[^"\\])*" | '(?:\\.|[^'\\])*' | //[^\r\n]* | /\*[\s\S]*?\*/
        | (?<![\w#])\[(?<column>[^\[\]\r\n]+)\]
        | (?<![\w#])\#(?<ordinal>[0-9]+)\b
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
    private static partial Regex LegacySyntax();

    public static IExpression Create(string code, Context context)
    {
        try
        {
            return Expression.Create(code, context);
        }
        catch (ExpressifSyntaxException)
        {
            var variables = new Dictionary<string, object?>();
            var variablePrefix = "nbiColumn";
            while (code.Contains($"@{variablePrefix}", StringComparison.OrdinalIgnoreCase))
                variablePrefix += "N";

            var updated = LegacySyntax().Replace(code, match =>
            {
                if (match.Groups["temporal"] is { Success: true } temporal)
                    return $"#\"{temporal.Value.Replace(' ', 'T')}\"";
                if (match.Groups["text"] is { Success: true } text)
                    return $"\"{text.Value}\"";
                if (match.Groups["column"] is { Success: true } column)
                    return AddColumnBinding(() => context.CurrentObject[column.Value]);
                if (match.Groups["ordinal"] is { Success: true } ordinal)
                {
                    var index = int.Parse(ordinal.Value, CultureInfo.InvariantCulture);
                    return AddColumnBinding(() => context.CurrentObject[index]);
                }

                return match.Value;
            });
            if (updated == code)
                throw;

            var expression = Expression.Create(updated, context);
            return variables.Count == 0 ? expression : expression.WithContext(new EvaluationContext(variables));

            string AddColumnBinding(Func<object?> getValue)
            {
                var name = variablePrefix + variables.Count.ToString(CultureInfo.InvariantCulture);
                variables.Add(name, getValue);
                return $"@{name}";
            }
        }
    }
}
