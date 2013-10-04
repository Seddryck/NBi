using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core
{
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

        public Result Compare(IEnumerable<string> x, IEnumerable<string> y, Comparison comparaison)
        {
            IEnumerable<string> missing = null;
            IEnumerable<string> unexpected = null;

            if (comparaison.HasFlag(Comparison.MissingItems))
                missing = x.Except(y, internalComparer).ToList();

            if (comparaison.HasFlag(Comparison.UnexpectedItems))
                unexpected = y.Except(x, internalComparer).ToList();

            var res = new Result(missing, unexpected);
            return res;
        }

        public class Result
        {
            public int MissingCount { get; private set; }
            public int UnexpectedCount { get; private set; }
            
            public IEnumerable<string> Missing { get; private set; }
            public IEnumerable<string> Unexpected { get; private set; }

            internal Result(IEnumerable<string> missing, IEnumerable<string> unexpected)
            {
                Missing = missing;
                MissingCount = missing==null ? 0 : missing.Count();
                Unexpected = unexpected;
                UnexpectedCount = unexpected==null ? 0 :unexpected.Count();
            }

            public void Sample()
            {
                Sample(ROWS_FOR_SAMPLE);
            }

            public void Sample(int count)
            {
                Missing = Missing.Take(count);
                Unexpected = Unexpected.Take(count);
            }
        }

        
    }
}
