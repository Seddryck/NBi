using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class HierarchyNotNullIfLevel : FilterNotNull
{

    internal HierarchyNotNullIfLevel(DiscoveryTarget target, IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Hierarchies, target, filters)
    {
    }

    protected override bool IsApplicable()
    {
        return GetSpecificFilter(DiscoveryTarget.Levels) != null || Target==DiscoveryTarget.Levels;
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("hierarchy");
    }
}
