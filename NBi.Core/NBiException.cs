using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class NBiException : Exception
    {
        public NBiException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
