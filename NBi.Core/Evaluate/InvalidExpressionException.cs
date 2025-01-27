using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate;

class InvalidExpressionException : NBiException
{
    public InvalidExpressionException(string message)
        : base(message)
    {

    }
}
