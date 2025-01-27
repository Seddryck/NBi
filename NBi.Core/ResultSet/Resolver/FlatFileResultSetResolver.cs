using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

class FlatFileResultSetResolver : IResultSetResolver
{
    private readonly FlatFileResultSetResolverArgs args;
    private readonly ServiceLocator serviceLocator;

    public FlatFileResultSetResolver(FlatFileResultSetResolverArgs args, ServiceLocator serviceLocator)
    {
        this.args = args;
        this.serviceLocator = serviceLocator;
    }

    public virtual IResultSet Execute()
    {
        var path = args.Path.Execute();
        var file = (Path.IsPathRooted(path)) ? path : args.BasePath + path;

        if (!IsFileExisting(file))
        {
            if (args.Redirection == null)
                throw new ExternalDependencyNotFoundException(file);
            else
                return args.Redirection.Execute();
        }
        else
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Loading data from flat file '{file}'");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var factory = serviceLocator.GetFlatFileReaderFactory();
        var reader = factory.Instantiate(args.ParserName, args.Profile);
        var dataTable = reader.ToDataTable(file);

        var rs = new DataTableResultSet(dataTable);
        stopWatch.Stop();
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to load data from flat file: {stopWatch.Elapsed:d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}");
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Result-set contains {rs.RowCount} row{(rs.RowCount > 1 ? "s" : string.Empty)} and {rs.ColumnCount} column{(rs.ColumnCount > 1 ? "s" : string.Empty)}");
        return rs;
    }

    protected virtual bool IsFileExisting(string fullpath) => File.Exists(fullpath);

}
