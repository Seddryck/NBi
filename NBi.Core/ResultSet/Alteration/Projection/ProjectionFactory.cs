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
            switch(args)
            {
                case ProjectAwayArgs x: return new ProjectAwayEngine(x);
                case ProjectArgs x: return new ProjectEngine(x);
                default: throw new ArgumentException();
            }
        }
    }
}
