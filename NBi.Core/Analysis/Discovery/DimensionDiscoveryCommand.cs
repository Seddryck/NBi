using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class DimensionDiscoveryCommand : PathDiscoveryCommand
    {
        public DimensionDiscoveryCommand(string connectionString, string perspectiveName, string path)
            : base(connectionString, perspectiveName, path)
        {
            Target = DiscoveryTarget.Hierarchies;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            Filter.DimensionUniqueName = GetUniqueNames()[0];
            Depth.Hierarchies = true;
        }
    }
}
