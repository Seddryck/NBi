using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Projection
{
    public class ProjectionFactory
    {
        public IProjectionEngine Instantiate(IProjectionArgs args)
        {
            return args switch
            {
                ProjectAwayArgs x => new ProjectAwayEngine(x),
                ProjectArgs x => new ProjectEngine(x),
                _ => throw new ArgumentException(),
            };
        }
    }
}
