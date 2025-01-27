using NBi.Core.Sequence.Resolver.Loop;

namespace NBi.Core.Sequence.Resolver;

public class SentinelLoopSequenceResolverArgs<T, U> : ISentinelLoopSequenceResolverArgs
{
    public T Seed { get; }
    public T Terminal { get; }
    public U Step { get; }
    public IntervalMode IntervalMode { get; }

    public SentinelLoopSequenceResolverArgs(T seed, T terminal, U step, IntervalMode intervalMode)
    {
        Seed = seed;
        Terminal = terminal;
        Step = step;
        IntervalMode = intervalMode;
    }
}