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
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceWarning, "!!!! NBiException !!!!");
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceWarning, message);
            if (innerException != null)
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceWarning, innerException.Message);
        }

        public NBiException(string message)
            : this(message, null)
        { }
    }
}
