using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class PathNotEmpty : DataNotEmpty
    {

        internal PathNotEmpty(string path)
            : base(path)
        {
            
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("path");
        }
    }
}
