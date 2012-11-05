using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
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
}
