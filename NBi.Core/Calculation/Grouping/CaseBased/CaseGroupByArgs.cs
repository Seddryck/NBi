using NBi.Core.Calculation.Asserting;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping.CaseBased;

public class CaseGroupByArgs : IGroupByArgs
{
    public IEnumerable<IPredication> Cases { get; set; }
    public Context Context { get; set; }

    public CaseGroupByArgs(IEnumerable<IPredication> cases, Context context)
        => (Cases, Context) = (cases, context);
}
