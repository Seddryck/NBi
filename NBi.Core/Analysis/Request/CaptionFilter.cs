using System;
using System.Linq;

namespace NBi.Core.Analysis.Request;

public class CaptionFilter: IFilter
{
    protected readonly string captionFilter;
    protected readonly DiscoveryTarget targetFilter;

    public CaptionFilter(string caption, DiscoveryTarget target)
    {
        captionFilter = caption;
        targetFilter = target;
    }

    public string Value { get { return captionFilter; } }
    public DiscoveryTarget Target { get { return targetFilter; } }

}
