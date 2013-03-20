using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal class HierarchyNotNull : FilterNotNull
    {

        internal HierarchyNotNull(string path)
            : base(path)
        {
            
        }

        internal override void GenerateException()
        {
            throw new DiscoveryRequestFactoryException("hierarchy");
        }
    }
}
