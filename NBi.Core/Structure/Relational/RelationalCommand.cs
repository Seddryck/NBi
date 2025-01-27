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

namespace NBi.Core.Structure.Relational;


class RelationalCommand : StructureDiscoveryCommand
{
    public RelationalCommand(IDbCommand command, IEnumerable<IPostCommandFilter> postFilters, CommandDescription description)
        : base(command, postFilters, description)
    {
    }

    public override IEnumerable<string> Execute()
    {
        var values = new List<RelationalRow>();

        (command.Connection ?? throw new InvalidOperationException()).Open();
        var rdr = ExecuteReader(command);
        while (rdr.Read())
        {
            var row = BuildRow(rdr);
            var isValidRow = true;

            foreach (var postFilter in postFilters)
                isValidRow &= postFilter.Evaluate(row);

            if (isValidRow)
                values.Add(row);
        }
        command.Connection.Close();

        return values.Select(v => v.Caption);
    }

    protected virtual RelationalRow BuildRow(IDataReader rdr)
    {
        var row = new RelationalRow(rdr.GetString(0));
        return row;
    }

    protected IDataReader ExecuteReader(IDbCommand cmd)
    {
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, cmd.CommandText);

        try
        {
            var rdr = cmd.ExecuteReader();
            return rdr;
        }
        catch (Exception ex)
        { throw new ConnectionException(ex, cmd.Connection?.ConnectionString ?? "Connection-string not set."); }
    }
}
