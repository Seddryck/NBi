using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Performance
{
    [SupportedCommandType(typeof(AdomdCommand))]
    internal class AdomdPerformanceEngine : DbCommandPerformanceEngine
    {
        protected internal AdomdPerformanceEngine(AdomdConnection connection, AdomdCommand command)
            : base(new AdomdExecutionEngine(connection, command))
        { }

        public override void CleanCache()
        {
            using var conn = engine.NewConnection();
            string xmla = string.Empty;
            using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream("NBi.Core.Query.Performance.CleanCache.xmla") ?? throw new FileNotFoundException())
            using (var reader = new StreamReader(stream))
                xmla = reader.ReadToEnd();

            engine.OpenConnection(conn);
            var csb = new DbConnectionStringBuilder() { ConnectionString = conn.ConnectionString };
            if (!csb.ContainsKey("Initial Catalog"))
                throw new ArgumentException("The token 'Initial Catalog' was not provided in the connection string due to this, it was impossible to clean the cache of the database.");


            var cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = string.Format(xmla, csb["Initial Catalog"]);
            cmd.ExecuteNonQuery();
        }
    }
}