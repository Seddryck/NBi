using NBi.Core.Sequence.Resolver.Loop;

namespace NBi.Core.Sequence.Resolver;

public class CountLoopSequenceResolverArgs<T, U> : ILoopSequenceResolverArgs
{
    public int Count { get; }
    public T Seed { get; }
    public U Step { get; }

    public CountLoopSequenceResolverArgs(int count, T seed, U step)
    {
        Count = count;
        Seed = seed;
        Step = step;
    }
}