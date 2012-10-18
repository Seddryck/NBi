using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class MeasureGroupNameNotEmpty : DataNotEmpty
    {

        internal MeasureGroupNameNotEmpty(string measureGroupName)
            : base(measureGroupName)
        {
            
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("measureGroupName");
        }
    }
}
