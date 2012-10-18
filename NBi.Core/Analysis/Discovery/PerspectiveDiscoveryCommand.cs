using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class PerspectiveDiscoveryCommand : CubeDiscoveryCommand
    {
        public string PerspectiveName { get; private set; }

        internal PerspectiveDiscoveryCommand(string connectionString, string perspectiveName)
            : base(connectionString)
        {
            PerspectiveName = perspectiveName;
            Target = DiscoveryTarget.Dimensions;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            Filter.Perspective = PerspectiveName;
            Depth.Dimensions = true;
            Depth.MeasureGroups = true;
        }
    }
}
