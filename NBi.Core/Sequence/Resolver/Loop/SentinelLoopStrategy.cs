using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

abstract class SentinelLoopStrategy<T, U> : ILoopStrategy<T>
{
    public SentinelLoopStrategy(T seed, T terminal, U step)
    {
        Terminal = terminal;
        Seed = seed;
        Step = step;
        CurrentValue = Seed;
        FirstLoop = true;
    }

    public T CurrentValue { get; set; }
    public T Terminal { get; }
    public T Seed { get; }
    public U Step { get; }
    protected bool FirstLoop { get; set; }

    object? ILoopStrategy.GetNext() => GetNext();
    public T GetNext()
    {
        if (FirstLoop)
        {
            FirstLoop = false;
            return Seed;
        }

        CurrentValue = GetNextValue(CurrentValue, Step);
        return CurrentValue;
    }

    protected abstract T GetNextValue(T previousValue, U step);

    public abstract bool IsOngoing();
}
