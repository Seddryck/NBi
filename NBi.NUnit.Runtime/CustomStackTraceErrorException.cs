using System;
using System.Linq;
using NBi.Core;

namespace NBi.NUnit.Runtime
{
    public class CustomStackTraceErrorException: TestException
    {
        private readonly string stackTrace;

        public CustomStackTraceErrorException(TestException ex, string stackTrace)
            : base(ex.Message)
        {
            this.stackTrace = stackTrace;
        }
        
        public override string StackTrace 
        { 
            get 
            {   
                return stackTrace;
            }
        }
    }
}
