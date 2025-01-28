using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request;

public class DiscoveryRequestFactoryException : ArgumentException
{
    public DiscoveryRequestFactoryException(string paramName) : base 
        (
            string.Format("You must fill the attribute '{0}'", paramName),
            paramName
        )
    {
        
    }

    public DiscoveryRequestFactoryException(string message, string paramName)
        : base
            (
            message,
            paramName
            )
    {

    }
}
