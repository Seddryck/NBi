using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Equivalence;

public class EquivalerException : NBiException
{
    public EquivalerException(string message)
        : base(message)
    {

    }
}
