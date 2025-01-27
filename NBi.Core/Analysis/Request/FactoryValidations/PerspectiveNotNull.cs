using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class PerspectiveNotNull : FilterNotNull
{

    internal PerspectiveNotNull(IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Perspectives, filters)
    {
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("Perspective");
    }
}
