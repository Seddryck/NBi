using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    interface IPredicate
    {
        bool Compare(object x, object y); 
    }
}
