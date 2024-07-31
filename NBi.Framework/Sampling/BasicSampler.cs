using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling
{
    class BasicSampler<T> : ISampler<T>
    {
        public int ThresholdSampleItem { get; }
        public int MaxSampleItem { get; }

        private bool isBuild = false;

        private IEnumerable<T> result = [];
        private bool isSampled = false;
        private int excludedRowCount = 0;

        public BasicSampler(int thresholdSampleItem, int maxSampleItem)
        {
            ThresholdSampleItem = thresholdSampleItem;
            MaxSampleItem = maxSampleItem;
        }

        public void Build(IEnumerable<T> fullSet)
        {
            result = fullSet.Take(fullSet.Count() > ThresholdSampleItem ? MaxSampleItem : fullSet.Count());
            isSampled = fullSet.Count() > ThresholdSampleItem;
            excludedRowCount = isSampled ? fullSet.Count() - MaxSampleItem : 0;
            isBuild = true;
        }

        public IEnumerable<T> GetResult()
        {
            if (!isBuild)
                throw new InvalidOperationException();
            return result;
        }

        public bool GetIsSampled()
        {
            if (!isBuild)
                throw new InvalidOperationException();
            return isSampled;
        }

        public int GetExcludedRowCount()
        {
            if (!isBuild)
                throw new InvalidOperationException();
            return excludedRowCount;
        }




    }
}
