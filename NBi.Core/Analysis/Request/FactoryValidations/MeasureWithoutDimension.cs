using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class MeasureWithoutDimension : ValidationFilter
{

    internal MeasureWithoutDimension(IEnumerable<IFilter> filters)
        : base(filters)
    {
    }

    internal override void Apply()
    {
        if (!(GetSpecificFilter(DiscoveryTarget.Dimensions) == null || GetSpecificFilter(DiscoveryTarget.Measures) == null))
            GenerateException();
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("Dimension and Measure cannot be specified at the same time");
    }
}
