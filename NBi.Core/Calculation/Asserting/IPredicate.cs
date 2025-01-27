using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Asserting;

public interface IPredicate
{
    bool Execute(object? x); 
}
