using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class LevelDiscoveryCommand : HierarchyDiscoveryCommand
    {
        public LevelDiscoveryCommand(string connectionString, string perspectiveName, string path)
            : base(connectionString, perspectiveName, path)
        {
            Target = DiscoveryTarget.Properties;
        }

        protected internal override void Initialize()
        {
            base.Initialize();
            Filter.DimensionUniqueName = GetUniqueNames()[2];
        }
    }
}
