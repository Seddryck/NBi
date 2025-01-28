using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request;

public class MetadataLinkedToDiscoveryRequest : MetadataDiscoveryRequest
{
    internal MetadataLinkedToDiscoveryRequest(string connectionString) 
        : base(connectionString)
    { }

    internal MetadataLinkedToDiscoveryRequest(string connectionString, DiscoveryTarget target, IEnumerable<IFilter> filters)
        : base(connectionString)
    {
        Target = target;
        AddFilters(filters);
    }
}
