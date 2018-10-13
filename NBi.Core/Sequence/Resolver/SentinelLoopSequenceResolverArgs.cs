using NBi.Core.Sequence.Resolver.Loop;

namespace NBi.Core.Sequence.Resolver
{
    public class SentinelLoopSequenceResolverArgs<T, U> : ILoopSequenceResolverArgs
    {
        public T Seed { get; }
        public T Terminal { get; }
        public U Step { get; }

        public SentinelLoopSequenceResolverArgs(T seed, T terminal, U step)
        {
            Seed = seed;
            Terminal = terminal;
            Step = step;
        }
    }
}