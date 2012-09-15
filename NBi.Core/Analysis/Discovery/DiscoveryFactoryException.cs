using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class DiscoveryFactoryException : ArgumentException
    {
        public DiscoveryFactoryException(string paramName) : base 
            (
                string.Format("You must fill the attribute '{0}'", paramName),
                paramName
            )
        {
            
        }

        public DiscoveryFactoryException(string message, string paramName)
            : base
                (
                message,
                paramName
                )
        {

        }
    }
}
