using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Core.Scalar.Resolver;
using System.Text.RegularExpressions;

namespace NBi.Core.Scalar.Format
{
    class InvariantFormatter : IFormatter
    {
        private ServiceLocator ServiceLocator { get; }
        private IDictionary<string, IVariable> Variables { get; }

        private const string SCALAR_PATTERN = @"{(@([\w\s\|\(\),-])+)[}:]";

        public InvariantFormatter(ServiceLocator serviceLocator, IDictionary<string, IVariable> variables)
            => (ServiceLocator, Variables) = (serviceLocator, variables);

        protected string Prepare(string text, out IList<IScalarResolverArgs> args)
        {
            var res = text;
            args = new List<IScalarResolverArgs>();
            var match = Regex.Match(text, SCALAR_PATTERN, RegexOptions.IgnoreCase);

            var i = 0;
            while (match.Success)
            {
                args.Add(BuildArgs(match.Value.Substring(1, match.Value.Length - 2)));
                res = res.Replace(match.Value, $"{{{i}{match.Value.Substring(match.Value.Length - 1)}");
                i += 1;
                match = match.NextMatch();
            }
            if (i == 0)
                throw new ArgumentException($"The text '{text}' doesn't contain any combination of formatter and variable. Don't you forget the curly braces '{{}}' or the arobas '@'?");

            return res;
        }

        protected IScalarResolverArgs BuildArgs(string text)
        {
            var factory = new ScalarResolverArgsFactory(ServiceLocator, new Context(Variables));
            return factory.Instantiate(text);
        }

        public string Execute(string text)
        {
            var newText = Prepare(text, out var args);

            var objects = new List<object?>();
            var factory = ServiceLocator.GetScalarResolverFactory();
            foreach (var arg in args)
            {
                var resolver = factory.Instantiate(arg);
                objects.Add(resolver.Execute());
            }

            var formatProvider = new CultureFactory().Invariant;

            return string.Format(formatProvider, newText, [.. objects]);
        }
    }
}
