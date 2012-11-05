using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class DimensionNotNull : FilterNotNull
    {

        internal DimensionNotNull(string path)
            : base(path)
        {
            
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("dimension");
        }
    }
}

