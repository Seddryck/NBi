using System;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class MetadataDiscoveryRequest : BaseDiscoveryRequest
    {
        public DiscoveryTarget Target { get; set; }

        public MetadataDiscoveryRequest() : base()
        {
        }

        private readonly string[] depths = new string[] { "dimension", "hierarchy", "level", "property" };

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
