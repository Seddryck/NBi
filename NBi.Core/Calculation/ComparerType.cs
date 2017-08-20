using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public enum ComparerType
    {
        LessThan = -2,
        LessThanOrEqual = -1,
        Equal = 0,
        MoreThanOrEqual = 1,
        MoreThan = 2,
        Null = 10,
        Empty = 11,
        NullOrEmpty = 12
    }
}
