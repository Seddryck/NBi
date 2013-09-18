using System;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core
{
    public class NBiException : Exception
    {
        public NBiException(string message, Exception innerException)
            : base(message, innerException)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceWarning, "!!!! NBiException !!!!");
            Trace.WriteLineIf(NBiTraceSwitch.TraceWarning, message);
            if (innerException != null)
                Trace.WriteLineIf(NBiTraceSwitch.TraceWarning, innerException.Message);
        }

        public NBiException(string message)
            : this(message, null)
        { }
    }
}
