using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public abstract class SampledFailureMessage<T> : FailureMessage
    {
        private readonly int maxItemCount;
        private readonly int sampleItemCount;

        public SampledFailureMessage() : this(10,15)
        { }
        public SampledFailureMessage(int sampleItemCount, int maxItemCount)
        {
            this.sampleItemCount = sampleItemCount;
            this.maxItemCount = maxItemCount;
        }

        protected IEnumerable<T> Sample(IEnumerable<T> fullSet)
        {
            return fullSet.Take(fullSet.Count() > maxItemCount ? sampleItemCount : fullSet.Count());
        }

        protected bool IsSampled(IEnumerable<T> fullSet)
        {
            return fullSet.Count() > maxItemCount;
        }

        protected int CountExcludedRows(IEnumerable<T> fullSet)
        {
            if (IsSampled(fullSet))
                return fullSet.Count() - sampleItemCount;
            else
                return 0;
        }

    }
}
