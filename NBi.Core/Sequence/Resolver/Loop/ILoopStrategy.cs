namespace NBi.Core.Sequence.Resolver.Loop;

public interface ILoopStrategy
{
    bool IsOngoing();
    object? GetNext();
}

public interface ILoopStrategy<T> : ILoopStrategy
{
    new T GetNext();
}