using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Connection
{
    class ConnectionWaitCommand : IDecorationCommandImplementation
    {
        private readonly IDbConnection connection;
        private readonly int timeOut;

        public IDbConnection Connection { get { return connection; } }
        public int TimeOut { get { return timeOut; } }

        public ConnectionWaitCommand(IDbConnection connection, int timeOut)
        {
            this.connection = connection;
            this.timeOut = timeOut;
        }

        public void Execute()
        {
            var stopWatch = new Stopwatch();
            var isConnectionAvailable = false;
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Will try to connect to '{0}' during {1} milli-seconds.", connection.ConnectionString, timeOut));
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < timeOut || isConnectionAvailable)
            {
                try
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Trying to connect to '{0}'.", connection.ConnectionString));
                    connection.Open();
                    connection.Close();
                    isConnectionAvailable = true;
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Successful connection to '{0}'.", connection.ConnectionString));
                }
                catch (Exception ex)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Fail to connect to '{0}': {1}", connection.ConnectionString, ex.Message));
                }
            }           
        }
    }
}
