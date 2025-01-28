using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request;

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
        return Target switch
        {
            DiscoveryTarget.Dimensions => depths[0],
            DiscoveryTarget.Hierarchies => depths[1],
            DiscoveryTarget.Levels => depths[2],
            DiscoveryTarget.Properties => depths[3],
            _ => string.Empty,
        };
    }

    public string GetNextDepthName()
    {
        return Target switch
        {
            DiscoveryTarget.Dimensions => depths[1],
            DiscoveryTarget.Hierarchies => depths[2],
            DiscoveryTarget.Levels => depths[3],
            _ => string.Empty,
        };
    }
   

}
