﻿using Microsoft.AnalysisServices.AdomdClient;
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

    class OlapCommand : StructureDiscoveryCommand
    {
        public OlapCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
            : base(command, postFilters, description)
        {
        }

        public override IEnumerable<string> Execute()
        {
            var values = new List<OlapRow>();

            command.Connection.Open();
            var rdr = ExecuteReader(command as AdomdCommand);
            while (rdr.Read())
            {
                var row = BuildRow(rdr);

                var isValid = true;
                foreach (var postFilter in postFilters)
                    isValid = postFilter.Evaluate(row);

                if (isValid)
                    values.Add(row);
            }
            command.Connection.Close();

            return values.Select(v => v.Caption);
        }

        protected virtual OlapRow BuildRow(AdomdDataReader rdr)
        {
            var row = new OlapRow();
            row.Caption = rdr.GetString(0);
            row.DisplayFolder = rdr.GetString(1);
            return row;
        }

        protected AdomdDataReader ExecuteReader(AdomdCommand cmd)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, cmd.CommandText);

            AdomdDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }
    }
}
