using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup.Violation;

public enum RowViolationState
{
    Missing = 1,
    Unexpected = 2,
    Mismatch = 3,
}
