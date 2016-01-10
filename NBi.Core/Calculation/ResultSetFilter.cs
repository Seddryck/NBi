using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public abstract class ResultSetFilter
    {
        private readonly static IResultSetFilter none = new NoneFilter();
        public static IResultSetFilter None
        {
            get { return none; }
        }
    }
}
