using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Equivalence;

[Flags]
public enum EquivalenceKind
{
    IntersectionOf = 0,
    SubsetOf = 1,
    SupersetOf = 2,
    EqualTo = 3
}
