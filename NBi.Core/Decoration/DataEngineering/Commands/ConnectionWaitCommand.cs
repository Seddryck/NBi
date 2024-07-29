using NBi.Core.Query.Client;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering.Commands
{
    class ConnectionWaitCommand : IDecorationCommand
    {
        private readonly ConnectionWaitCommandArgs args;

        public ConnectionWaitCommand(ConnectionWaitCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.ConnectionString, args.TimeOut.Execute());

        internal void Execute(string connectionString, int timeOut)
        {
            var stopWatch = new Stopwatch();
            var isConnectionAvailable = false;
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Will try to connect to '{connectionString}' during {timeOut} milli-seconds.");
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < timeOut && !isConnectionAvailable)
            {
                try
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Building connection string with '{connectionString}'.");
                    var sessionFactory = new ClientProvider();
                    var connection = sessionFactory.Instantiate(connectionString).CreateNew() as IDbConnection;

                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Trying to connect to '{connection!.ConnectionString}'.");
                    connection.Open();
                    connection.Close();
                    isConnectionAvailable = true;
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Successful connection to '{connection.ConnectionString}'.");
                }
                catch (Exception ex)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Fail to connect to '{connectionString}': {ex.Message}");
                }
            }        
   
            if (!isConnectionAvailable)
                throw new NBiException($"The connection to '{connectionString}' wasn't available after {timeOut} milli-seconds: timeout reached!");
        }
    }
}
