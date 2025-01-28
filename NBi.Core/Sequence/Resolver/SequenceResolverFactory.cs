using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver.Loop;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class SequenceResolverFactory
{
    private ServiceLocator ServiceLocator { get; }
    public SequenceResolverFactory(ServiceLocator serviceLocator)
        => ServiceLocator = serviceLocator;
    
    internal ISequenceResolver<T> Instantiate<T>(ISequenceResolverArgs args)
    {
        switch (args)
        {
            case QuerySequenceResolverArgs queryArgs: return new QuerySequenceResolver<T>(queryArgs, ServiceLocator);
            case ListSequenceResolverArgs listArgs: return new ListSequenceResolver<T>(listArgs);
            case CustomSequenceResolverArgs customArgs: return new CustomSequenceResolver<T>(customArgs);
            case FileLoopSequenceResolverArgs fileArgs: return (ISequenceResolver<T>)new FileLoopSequenceResolver(fileArgs);
            case ILoopSequenceResolverArgs loopArgs:
                {
                    var strategy = MapStrategy<T>(loopArgs);
                    return new LoopSequenceResolver<T>(strategy);
                }
            case FilterSequenceResolverArgs filterArgs: return new FilterSequenceResolver<T>(filterArgs);
            default:
                throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Sequence");
        }
    }

    public ISequenceResolver Instantiate(ColumnType type, ISequenceResolverArgs args)
    {
        return type switch
        {
            ColumnType.Text => Instantiate<string>(args),
            ColumnType.Numeric => Instantiate<decimal>(args),
            ColumnType.DateTime => Instantiate<DateTime>(args),
            ColumnType.Boolean => Instantiate<bool>(args),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private ILoopStrategy MapStrategy<T>(ILoopSequenceResolverArgs args)
    {
        return args switch
        {
            CountLoopSequenceResolverArgs<decimal, decimal> x => new CountNumericLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy,
            CountLoopSequenceResolverArgs<DateTime, IDuration> x => new CountDateTimeLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy,
            SentinelLoopSequenceResolverArgs<decimal, decimal> x => x.IntervalMode switch
            {
                IntervalMode.Close => new SentinelCloseNumericLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy,
                IntervalMode.HalfOpen => new SentinelHalfOpenNumericLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy,
                _ => throw new ArgumentOutOfRangeException(),
            },
            SentinelLoopSequenceResolverArgs<DateTime, IDuration> x => x.IntervalMode switch
            {
                IntervalMode.Close => new SentinelCloseDateTimeLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy,
                IntervalMode.HalfOpen => new SentinelHalfOpenDateTimeLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy,
                _ => throw new ArgumentOutOfRangeException(),
            },
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
