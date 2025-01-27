using NBi.Core.ResultSet.Lookup.Violation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup;

public interface ILookupAnalyzer
{
    LookupViolationCollection Execute(object candidate, object reference);
}
