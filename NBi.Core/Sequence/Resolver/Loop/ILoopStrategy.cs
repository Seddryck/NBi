namespace NBi.Core.Sequence.Resolver.Loop
{
    public interface ILoopStrategy
    { }

    public interface ILoopStrategy<T> : ILoopStrategy
    {
        T GetNext();
        bool IsOngoing();
    }
}