using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap;


class DistinctOlapCommand : OlapCommand
{
    public DistinctOlapCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
        : base(command, postFilters, description)
    {
    }

    public override IEnumerable<string> Execute()
    {
        var values = base.Execute();
        return values.Distinct();
    }
}
