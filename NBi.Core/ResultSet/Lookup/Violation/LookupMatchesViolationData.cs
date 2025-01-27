using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup.Violation;

public class LookupMatchesViolationData
{
    public bool IsEqual { get; private set; }
    public object Value { get; private set; }
    public LookupMatchesViolationData(bool isEqual, object value) 
        => (IsEqual, Value) = (isEqual, value);
}
