using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class TableNotNullIfColumn : FilterNotNull
{

    internal TableNotNullIfColumn(DiscoveryTarget target,IEnumerable<IFilter> filters)
        : base(DiscoveryTarget.Tables, target, filters)
    {
    }

    protected override bool IsApplicable()
    {
        return GetSpecificFilter(DiscoveryTarget.Columns) != null || Target == DiscoveryTarget.Columns;
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("table");
    }
}

