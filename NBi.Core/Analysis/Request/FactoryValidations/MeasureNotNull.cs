using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class MeasureNotNull : FilterNotNull
{

    internal MeasureNotNull(IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Measures, filters)
    {
    }

    protected override bool IsApplicable()
    {
        return (GetSpecificFilter(DiscoveryTarget.DisplayFolders) != null && GetSpecificFilter(DiscoveryTarget.Dimensions) == null)
            && Target!=DiscoveryTarget.Measures;
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("measure");
    }
}

