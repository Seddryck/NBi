using NBi.Core.Injection;
using NBi.Core.Sequence.Resolver.Loop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class SequenceResolverFactory
    {
        private readonly ServiceLocator serviceLocator;
        public SequenceResolverFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ISequenceResolver<T> Instantiate<T>(ISequenceResolverArgs args)
        {
            if (args is ListSequenceResolverArgs)
                return new ListSequenceResolver<T>((ListSequenceResolverArgs)args);
            if (args is ILoopSequenceResolverArgs)
            {
                    ILoopStrategy<T> strategy = null;
                    if (args is CountLoopSequenceResolverArgs<decimal, decimal>)
                        strategy = new CountNumericLoopStrategy((args as CountLoopSequenceResolverArgs<decimal, decimal>).Count, (args as CountLoopSequenceResolverArgs<decimal, decimal>).Seed, (args as CountLoopSequenceResolverArgs<decimal, decimal>).Step) as ILoopStrategy<T>;
                    else if (args is SentinelLoopSequenceResolverArgs<decimal, decimal>)
                        strategy = new SentinelNumericLoopStrategy((args as SentinelLoopSequenceResolverArgs<decimal, decimal>).Seed, (args as SentinelLoopSequenceResolverArgs<decimal, decimal>).Terminal, (args as SentinelLoopSequenceResolverArgs<decimal, decimal>).Step) as ILoopStrategy<T>;
                    else if (args is CountLoopSequenceResolverArgs<DateTime, TimeSpan>)
                        strategy = new CountDateTimeLoopStrategy((args as CountLoopSequenceResolverArgs<DateTime, TimeSpan>).Count, (args as CountLoopSequenceResolverArgs<DateTime, TimeSpan>).Seed, (args as CountLoopSequenceResolverArgs<DateTime, TimeSpan>).Step) as ILoopStrategy<T>;
                    else if (args is SentinelLoopSequenceResolverArgs<DateTime, TimeSpan>)
                        strategy = new SentinelDateTimeLoopStrategy((args as SentinelLoopSequenceResolverArgs<DateTime, TimeSpan>).Seed, (args as SentinelLoopSequenceResolverArgs<DateTime, TimeSpan>).Terminal, (args as SentinelLoopSequenceResolverArgs<DateTime, TimeSpan>).Step) as ILoopStrategy<T>;

                    return new LoopSequenceResolver<T>(strategy);
            }
            return new ListSequenceResolver<T>((ListSequenceResolverArgs)args);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
        }
    }
}
