using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup
{
    public class LookupFactory
    {
        public ILookupEngine Instantiate(ILookupArgs args)
        {
            switch(args)
            {
                case LookupReplaceArgs x: return new LookupReplaceEngine(x);
                default: throw new ArgumentException();
            }
        }
    }
}
