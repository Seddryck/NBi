using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class MetadataDiscoveryRequest : BaseDiscoveryRequest
    {
        public DiscoveryTarget Target { get; set; }

        internal MetadataDiscoveryRequest(string connectionString) 
            : base(connectionString)
        { }

        internal MetadataDiscoveryRequest(string connectionString, DiscoveryTarget target, IEnumerable<IFilter> filters)
            : base(connectionString)
        {
            Target = target;
            AddFilters(filters);
        }

        private readonly string[] depths = ["dimension", "hierarchy", "level", "property"];

        public string GetDepthName()
        {
            switch (Target)
            {
                case DiscoveryTarget.Dimensions:
                    return depths[0];
                case DiscoveryTarget.Hierarchies:
                    return depths[1];
                case DiscoveryTarget.Levels:
                    return depths[2];
                case DiscoveryTarget.Properties:
                    return depths[3];
                default:
                    return string.Empty;
            }
        }

        public string GetNextDepthName()
        {
            switch (Target)
            {
                case DiscoveryTarget.Dimensions:
                    return depths[1];
                case DiscoveryTarget.Hierarchies:
                    return depths[2];
                case DiscoveryTarget.Levels:
                    return depths[3];
                default:
                    return string.Empty;
            }
        }
       

    }
}
