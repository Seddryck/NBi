using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling
{
    class NoneSampler<T> : ISampler<T>
    {
        private bool isBuild = false;

        private IEnumerable<T> result = [];
        private int excludedRowCount = 0;

        public NoneSampler()
        {
        }

        public void Build(IEnumerable<T> fullSet)
        {
            result = new List<T>(0);
            excludedRowCount = fullSet.Count();
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
            return true;
        }

        public int GetExcludedRowCount()
        {
            if (!isBuild)
                throw new InvalidOperationException();
            return excludedRowCount;
        }




    }
}
