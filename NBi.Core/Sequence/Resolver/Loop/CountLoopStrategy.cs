using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

abstract class CountLoopStrategy<T, U> : ILoopStrategy<T>
{
    public CountLoopStrategy(int count, T seed, U step)
    {
        Count = count;
        Seed = seed;
        Step = step;
        CurrentValue = Seed;
    }

    public int LoopCount { get; set; }
    public T CurrentValue { get; set; }
    public int Count { get; }
    public T Seed { get; }
    public U Step { get; }


    object? ILoopStrategy.GetNext() => GetNext();
    public T GetNext()
    {
        if (LoopCount == 0)
        {
            LoopCount = 1;
            return Seed;
        }

        CurrentValue = GetNextValue(CurrentValue, Step);
        LoopCount +=1;
        return CurrentValue;
    }

    protected abstract T GetNextValue(T previousValue, U step);

    public bool IsOngoing() => LoopCount < Count;
}




