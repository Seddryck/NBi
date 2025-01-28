using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness;

public class ResultOccurenceUniqueRows
{
    public KeyCollection Keys { get; private set; }
    public int OccurenceCount { get; private set; }

    public ResultOccurenceUniqueRows(KeyCollection keys, int occurenceCount)
    {
        OccurenceCount = occurenceCount;
        Keys = keys;
    }
}
