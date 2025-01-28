using NBi.Core.Injection;
using NBi.Core.Scalar.Format;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class FormatScalarResolver : IScalarResolver<string>
{
    private readonly FormatScalarResolverArgs args;
    private readonly ServiceLocator serviceLocator;

    public FormatScalarResolver(FormatScalarResolverArgs args, ServiceLocator serviceLocator)
    {
        this.args = args;
        this.serviceLocator = serviceLocator;
    }

    protected IFormatter ResolveFormatter()
    {
        var factory = serviceLocator.GetFormatterFactory();
        var formatter = factory.Instantiate(args.Context);
        return formatter;
    }

    public string Execute()
    {
        var formatter = ResolveFormatter();
        var value = formatter.Execute(args.Text);
        return value;
    }

    object IResolver.Execute() => Execute();
}
