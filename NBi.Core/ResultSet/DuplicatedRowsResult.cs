using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class DuplicatedRowsResult
    {
        public bool HasNoDuplicate { get; private set; }
        public int RowCount { get; private set; }
        public IEnumerable<DuplicatedRowsResultOccurence> Values { get; private set; }
        
        public DuplicatedRowsResult(int count, IEnumerable<KeyValuePair<KeyCollection, int>> values)
        {
            RowCount = count;
            Values = values.Select(x => new DuplicatedRowsResultOccurence(x.Key, x.Value)).OrderByDescending(x => x.OccurenceCount);
            HasNoDuplicate = values.Count() == 0;
        }
    }

    public class DuplicatedRowsResultOccurence
    {
        public KeyCollection Keys { get; private set; }
        public int OccurenceCount { get; private set; }

        public DuplicatedRowsResultOccurence(KeyCollection keys, int occurenceCount)
        {
            OccurenceCount = occurenceCount;
            Keys = keys;
        }
    }




}
