using NBi.Core.Injection;
using NBi.Core.Scalar.Duration;
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
                var strategy = MapStrategy<T>(args as ILoopSequenceResolverArgs);
                return new LoopSequenceResolver<T>(strategy);
            }
            return new ListSequenceResolver<T>((ListSequenceResolverArgs)args);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
        }

        private ILoopStrategy MapStrategy<T>(ILoopSequenceResolverArgs args)
        {
            switch (args)
            {
                case CountLoopSequenceResolverArgs<decimal, decimal> x:
                    return new CountNumericLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy;
                case SentinelLoopSequenceResolverArgs<decimal, decimal> x:
                    return new SentinelNumericLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                case CountLoopSequenceResolverArgs<DateTime, IDuration> x:
                    return new CountDateTimeLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy;
                case SentinelLoopSequenceResolverArgs<DateTime, IDuration> x:
                    return new SentinelDateTimeLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
