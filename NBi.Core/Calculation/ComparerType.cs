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
        NullOrEmpty = 12,
        StartsWith = 20,
        EndsWith = 21,
        Contains = 22,
        MatchesRegex = 23,
        UpperCase = 30,
        LowerCase = 31,
        WithinRange = 40,
    }
}
