using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public class ScalarResolverArgsFactory
    {
        private ServiceLocator ServiceLocator { get; }
        private Context Context { get; }

        public ScalarResolverArgsFactory(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public IScalarResolverArgs Instantiate(string value)
        {
            switch (value)
            {
                case string obj when obj.TrimStart().StartsWith("`") && obj.TrimEnd().EndsWith("`"):
                    return new LiteralScalarResolverArgs(obj.Trim()[1..^1]);
                case string obj when string.IsNullOrEmpty(value): return new LiteralScalarResolverArgs(string.Empty);
                case null: return new LiteralScalarResolverArgs(string.Empty);
                default:
                    var tokens = Regex.Matches(value, @"(?:(?:\{(?>[^{}]+|\{(?<subpart>)|\}(?<-subpart>))*(?(subpart)(?!))\})|[^|])+").Cast<Match>().Select(x => x.Value.Trim());
                    var firstToken = tokens.First().Trim();
                    var prefix = tokens.First().Trim().ToCharArray()[0];
                    var suffix = tokens.Last().Trim().ToCharArray().Last();
                    var functions = tokens.Skip(1);
                    var factory = ServiceLocator.GetScalarResolverFactory();

                    var columnIdentifierFactory = new ColumnIdentifierFactory();

                    IScalarResolverArgs args = prefix switch
                    {
                        '@' => new GlobalVariableScalarResolverArgs(firstToken[1..], Context),
                        '~' => new FormatScalarResolverArgs(firstToken[1..], Context),
                        '[' when firstToken.ToCharArray().Last() == ']'
                                && !firstToken.Contains(';')
                                && MatchExternalBrakets(firstToken).Count == 1
                                => new ContextScalarResolverArgs(Context, columnIdentifierFactory.Instantiate(firstToken)),
                        '#' => new ContextScalarResolverArgs(Context, columnIdentifierFactory.Instantiate(firstToken)),
                        _ => new LiteralScalarResolverArgs(firstToken)
                    };

                    if (functions.Any())
                    {
                        var transformations = new List<INativeTransformation>();
                        var nativeTransformationFactory = new NativeTransformationFactory(ServiceLocator, Context);
                        foreach (var function in functions)
                            transformations.Add(nativeTransformationFactory.Instantiate(function));

                        return new FunctionScalarResolverArgs(factory.Instantiate(args), transformations);
                    }
                    else
                        return args;
            }
        }

        private MatchCollection MatchExternalBrakets(string value)
        {
            var regex = new Regex(@"
                            \[                    # Match [
                            (
                                [^[\]]+           # all chars except []
                                | (?<Level>\[)    # or if [ then Level += 1
                                | (?<-Level>\])   # or if ] then Level -= 1
                            )+                    # Repeat (to go from inside to outside)
                            (?(Level)(?!))        # zero-width negative lookahead assertion
                            \]                    # Match ]",
                            RegexOptions.IgnorePatternWhitespace);
            return regex.Matches(value);
        }
    }
}
