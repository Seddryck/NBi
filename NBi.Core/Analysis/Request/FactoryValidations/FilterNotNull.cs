using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal abstract class FilterNotNull : ValidationFilter
{
    private readonly DiscoveryTarget target;
    private readonly DiscoveryTarget filterInvestigated;

    protected DiscoveryTarget Target 
    {
        get
        {
            return target;
        }
    }

    internal FilterNotNull(DiscoveryTarget filterInvestigated, IEnumerable<IFilter> filters)
        : this(filterInvestigated, filterInvestigated, filters)
    {
        //this.target = target;
    }

    internal FilterNotNull(DiscoveryTarget filterInvestigated, DiscoveryTarget target, IEnumerable<IFilter> filters)
        : base(filters)
    {
        this.target = target;
        this.filterInvestigated = filterInvestigated;
    }

    internal override void Apply()
    {
        if (IsApplicable())
        {
            var filter = GetSpecificFilter(filterInvestigated, Filters);

            if (filter==null)
                GenerateException();
        }
    }

    protected virtual bool IsApplicable()
    {
        return true;
    }


}
