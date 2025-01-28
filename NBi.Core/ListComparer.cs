using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core;

public class ListComparer
{
    private const int ROWS_FOR_SAMPLE = 10;

    [Flags]
    public enum Comparison
    {
        MissingItems = 1,
        UnexpectedItems = 2,
        Both = 3
    }

    private readonly IEqualityComparer<string> internalComparer;

    public ListComparer()
    {
        internalComparer = StringComparer.InvariantCultureIgnoreCase;
    }

    public Result Compare(IEnumerable<string> x, IEnumerable<string> y)
    {
        return Compare(x, y, Comparison.Both);
    }

    public Result Compare(IEnumerable<string>? x, IEnumerable<string>? y, Comparison comparaison)
    {
        IEnumerable<string>? missing = null;
        IEnumerable<string>? unexpected = null;
        if (x is null && y is null)
            return new Result(null, null);

        if (x is null && y is not null)
            return new Result(null, y);

        if (x is not null && y is null)
            return new Result(x, null);

        if (comparaison.HasFlag(Comparison.MissingItems))
            missing = x!.Except(y!, internalComparer).ToList();

        if (comparaison.HasFlag(Comparison.UnexpectedItems))
            unexpected = y!.Except(x!, internalComparer).ToList();

        var res = new Result(missing, unexpected);
        return res;
    }

    public class Result
    {
        public int MissingCount { get; private set; }
        public int UnexpectedCount { get; private set; }

        public IEnumerable<string> Missing { get; private set; }
        public IEnumerable<string> Unexpected { get; private set; }

        public Result(IEnumerable<string>? missing, IEnumerable<string>? unexpected)
        {
            Missing = missing ?? [];
            MissingCount = missing == null ? 0 : missing.Count();
            Unexpected = unexpected ?? [];
            UnexpectedCount = unexpected == null ? 0 : unexpected.Count();
        }

        public Result Sample()
        {
            return Sample(ROWS_FOR_SAMPLE);
        }

        public Result Sample(int count)
        {
            if (Missing != null)
                Missing = Missing.Take(count);

            if (Unexpected != null)
                Unexpected = Unexpected.Take(count);

            return this;
        }
    }


}
