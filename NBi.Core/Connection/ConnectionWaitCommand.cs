using NBi.Core.Query.Session;
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
        private readonly string connectionString;
        private readonly int timeOut;

        public string ConnectionString { get { return connectionString; } }
        public int TimeOut { get { return timeOut; } }

        public ConnectionWaitCommand(string connectionString, int timeOut)
        {
            this.connectionString = connectionString;
            this.timeOut = timeOut;
        }

        public void Execute()
        {
            var stopWatch = new Stopwatch();
            var isConnectionAvailable = false;
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, String.Format("Will try to connect to '{0}' during {1} milli-seconds.", connectionString, timeOut));
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < timeOut && !isConnectionAvailable)
            {
                try
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Building connection string with '{0}'.", connectionString));
                    var sessionFactory = new SessionProvider();
                    var connection = sessionFactory.Instantiate(connectionString).CreateNew() as IDbConnection;

                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Trying to connect to '{0}'.", connection.ConnectionString));
                    connection.Open();
                    connection.Close();
                    isConnectionAvailable = true;
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Successful connection to '{0}'.", connection.ConnectionString));
                }
                catch (Exception ex)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, String.Format("Fail to connect to '{0}': {1}", connectionString, ex.Message));
                }
            }        
   
            if (!isConnectionAvailable)
                throw new NBiException(String.Format("The connection to '{0}' wasn't available after {1} milli-seconds: timeout reached!", connectionString, timeOut));
        }
    }
}
