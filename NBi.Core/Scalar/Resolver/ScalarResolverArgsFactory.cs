using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
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
                    return new LiteralScalarResolverArgs(obj.Trim().Substring(1, obj.Trim().Length - 2));
                case string obj when string.IsNullOrEmpty(value): return new LiteralScalarResolverArgs(string.Empty);
                case null: return new LiteralScalarResolverArgs(string.Empty);
                default:
                    var tokens = Regex.Matches(value, @"(?:(?:\{(?>[^{}]+|\{(?<subpart>)|\}(?<-subpart>))*(?(subpart)(?!))\})|[^|])+").Cast<Match>().Select(x => x.Value.Trim());
                    var firstToken = tokens.First().Trim();
                    var prefix = tokens.First().Trim().ToCharArray()[0];
                    var suffix = tokens.Last().Trim().ToCharArray().Last();
                    var functions = tokens.Skip(1);
                    var factory = ServiceLocator.GetScalarResolverFactory();
                    IScalarResolverArgs args = null;

                    var columnIdentifierFactory = new ColumnIdentifierFactory();

                    switch (prefix)
                    {
                        case '@':
                            args = new GlobalVariableScalarResolverArgs(firstToken.Substring(1), Context?.Variables);
                            break;
                        case '~':
                            args = new FormatScalarResolverArgs(firstToken.Substring(1), Context?.Variables);
                            break;
                        case '[' when suffix==']' && !tokens.Any(x => x.Contains(';')):
                        case '#':
                            args = new ContextScalarResolverArgs(Context, columnIdentifierFactory.Instantiate(firstToken));
                            break;
                        default:
                            args = new LiteralScalarResolverArgs(firstToken);
                            break;
                    }

                    if (functions.Count() > 0)
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
    }
}
