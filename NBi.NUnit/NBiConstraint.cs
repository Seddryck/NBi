using NBi.Framework;
using NBi.Framework.FailureMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public abstract class NBiConstraint : NUnitCtr.Constraint
    {
        public ITestConfiguration Configuration {get; set;}

        public NBiConstraint()
        {
        }
    }
}
