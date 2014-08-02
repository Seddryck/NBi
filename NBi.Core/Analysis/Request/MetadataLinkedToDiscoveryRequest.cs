using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class MetadataLinkedToDiscoveryRequest : MetadataDiscoveryRequest
    {
        internal MetadataLinkedToDiscoveryRequest() : base()
        {
        }

        internal MetadataLinkedToDiscoveryRequest(string connectionString, DiscoveryTarget target, IEnumerable<IFilter> filters)
            : base()
        {
            base.ConnectionString = connectionString;
            this.Target = target;
            base.AddFilters(filters);
        }
    }
}
