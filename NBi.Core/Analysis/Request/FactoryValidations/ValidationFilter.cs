using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal abstract class ValidationFilter : Validation
{
    private readonly IEnumerable<IFilter> filters;

    protected IEnumerable<IFilter> Filters
    {
        get
        {
            return filters;
        }
    }

    protected ValidationFilter(IEnumerable<IFilter> filters)
        : base()
    {
        this.filters = filters;
    }

    protected IFilter? GetSpecificFilter(DiscoveryTarget discoveryTarget, IEnumerable<IFilter> filters)
    {
        return filters.FirstOrDefault(f => f.Target == discoveryTarget);
    }

    protected IFilter? GetSpecificFilter(DiscoveryTarget discoveryTarget)
    {
        return GetSpecificFilter(discoveryTarget, Filters);
    }

}
