using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class HierarchyDiscoveryCommand : DimensionDiscoveryCommand
    {
        public HierarchyDiscoveryCommand(string connectionString, string perspectiveName, string path)
            : base(connectionString, perspectiveName, path)
        {
            Target = DiscoveryTarget.Levels;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            Filter.HierarchyUniqueName = GetUniqueNames()[1];
            Depth.Levels = true;
        }
    }
}
