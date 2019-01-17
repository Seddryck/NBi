using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public interface ILookupAnalyzer
    {
        LookupViolations Execute(object candidate, object reference);
    }
}
