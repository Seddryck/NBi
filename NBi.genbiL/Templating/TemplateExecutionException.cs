using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Templating;

public class TemplateExecutionException : Exception
{
    public TemplateExecutionException(string message): base(message)
    {
        
    }
}
