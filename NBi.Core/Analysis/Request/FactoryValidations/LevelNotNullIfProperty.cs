using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class LevelNotNullIfProperty : FilterNotNull
{

    internal LevelNotNullIfProperty(DiscoveryTarget target, IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Levels, target, filters)
    {
    }

    protected override bool IsApplicable()
    {
        return GetSpecificFilter(DiscoveryTarget.Properties) != null || Target==DiscoveryTarget.Properties;
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("level");
    }
}
