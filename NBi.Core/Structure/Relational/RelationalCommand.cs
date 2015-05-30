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

namespace NBi.Core.Structure.Relational
{

    class RelationalCommand : StructureDiscoveryCommand
    {
        public RelationalCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
            : base(command, postFilters, description)
        {
        }

        public override IEnumerable<string> Execute()
        {
            var values = new List<RelationalRow>();

            command.Connection.Open();
            var rdr = ExecuteReader(command);
            while (rdr.Read())
            {
                var row = new RelationalRow();
                row.Caption = rdr.GetString(0);

                foreach (var postFilter in postFilters)
                    if (postFilter.Evaluate(row))
                        continue;

                values.Add(row);
            }
            command.Connection.Close();

            return values.Select(v => v.Caption);
        }

        protected IDataReader ExecuteReader(IDbCommand cmd)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, cmd.CommandText);

            IDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (Exception ex)
            { throw new ConnectionException(ex, cmd.Connection.ConnectionString); }
        }
    }
}
