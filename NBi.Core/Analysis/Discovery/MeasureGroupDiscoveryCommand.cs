using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class MeasureGroupDiscoveryCommand : PerspectiveDiscoveryCommand
    {
        public string MeasureGroupName { get; private set; }

        public MeasureGroupDiscoveryCommand(string connectionString, string perspectiveName, string measureGroupName)
            : base(connectionString, perspectiveName)
        {
            MeasureGroupName = measureGroupName;
            Target = DiscoveryTarget.Measures;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            Filter.MeasureGroupName = MeasureGroupName;
            Depth.Measures = true;
            Depth.Dimensions = false;
        }
    }
}
