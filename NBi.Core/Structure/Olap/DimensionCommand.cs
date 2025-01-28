using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap;


class DimensionCommand : OlapCommand
{
    protected internal DimensionCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
        : base(command, postFilters, description)
    { } 

    protected override OlapRow BuildRow(AdomdDataReader rdr)
    {
        var row = new DimensionRow(rdr.GetString(0), rdr.GetString(1), rdr.GetInt16(2));
        return row;
    }

}
