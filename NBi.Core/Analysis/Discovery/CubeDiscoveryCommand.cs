using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    public class CubeDiscoveryCommand : DiscoveryCommand
    {
        internal CubeDiscoveryCommand(string connectionString)
            : base(connectionString)
        {
            Target = DiscoveryTarget.Perspectives;
        }

        protected internal override void Initialize()
        {
            Depth.Perspectives = true;
        }
    }
}
