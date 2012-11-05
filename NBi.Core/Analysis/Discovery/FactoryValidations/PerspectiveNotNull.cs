using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class PerspectiveNotNull : FilterNotNull
    {

        internal PerspectiveNotNull(string perspectiveName)
            : base(perspectiveName)
        {
            
            
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("perspectiveName");
        }
    }
}
