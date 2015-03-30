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

namespace NBi.Core.Structure.Olap
{

    class MeasureGroupRelationCommand : OlapCommand
    {
        protected internal MeasureGroupRelationCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
            : base(command, postFilters, description)
        {
        } 

        protected override OlapRow BuildRow(AdomdDataReader rdr)
        {
            var row = new OlapRow();
            row.Caption = rdr.GetString(0).Substring(1, rdr.GetString(0).Length - 2);
            row.DisplayFolder = rdr.GetString(1);
            return row;
        }

    }
}
