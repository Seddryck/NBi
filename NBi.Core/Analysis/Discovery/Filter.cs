using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class Filter
    {
        public string Perspective { get; internal set; }
        public string DimensionUniqueName { get; internal set; }
        public string HierarchyUniqueName { get; internal set; }
        public string LevelUniqueName { get; internal set; }
        public string MeasureGroupName { get; internal set; }

        public static Filter Empty
        {
            get { return new Filter(); }
        }
    }
}
