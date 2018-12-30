using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public abstract class ResultSetFilter
    {
        public static IResultSetFilter None { get; } = new NoneFilter();
    }
}
