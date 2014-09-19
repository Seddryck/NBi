using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Service
{
    public class TemplateExecutionException : Exception
    {
        public TemplateExecutionException(string message): base(message)
        {
            
        }
    }
}
