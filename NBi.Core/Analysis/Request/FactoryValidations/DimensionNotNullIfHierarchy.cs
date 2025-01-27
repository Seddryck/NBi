using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class DimensionNotNullIfHierarchy : FilterNotNull
{

    internal DimensionNotNullIfHierarchy(DiscoveryTarget target, IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Dimensions, target, filters)
    {
    }

    protected override bool IsApplicable()
    {
        return GetSpecificFilter(DiscoveryTarget.Hierarchies) != null || Target == DiscoveryTarget.Hierarchies;
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("dimension");
    }
}

