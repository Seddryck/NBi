using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class Depth
    {
        public bool Perspectives { get; internal set; }
        public bool Dimensions { get; internal set; }
        public bool Hierarchies { get; internal set; }
        public bool Levels { get; internal set; }
        public bool MeasureGroups { get; internal set; }
        public bool Measures { get; internal set; }
    }
}
