﻿using NBi.Core.Injection;
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

namespace NBi.Core.ResultSet.Resolver
{
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

            var rs = new ResultSet();
            rs.Load(dataTable);
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to load data from flat file: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Result-set contains {dataTable.Rows.Count} row{(dataTable.Rows.Count > 1 ? "s" : string.Empty)} and {dataTable.Columns.Count} column{(dataTable.Columns.Count > 1 ? "s" : string.Empty)}");
            return rs;
        }

        protected virtual bool IsFileExisting(string fullpath) => File.Exists(fullpath);

    }
}
