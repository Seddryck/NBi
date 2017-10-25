using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class UniqueRowsResultOccurence
    {
        public KeyCollection Keys { get; private set; }
        public int OccurenceCount { get; private set; }

        public UniqueRowsResultOccurence(KeyCollection keys, int occurenceCount)
        {
            OccurenceCount = occurenceCount;
            Keys = keys;
        }
    }
}
