using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Reshaping
{
    public class ReshapingFactory
    {
        public IReshapingEngine Instantiate(IReshapingArgs args)
        {
            switch(args)
            {
                case UnstackArgs x: return new UnstackEngine(x);
                default: throw new ArgumentException();
            }
        }
    }
}
