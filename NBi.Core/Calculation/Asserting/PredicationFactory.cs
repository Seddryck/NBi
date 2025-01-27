using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Asserting;

internal class PredicationFactory
{
    public IPredication Instantiate(IPredicate predicate, IColumnIdentifier identifier)
        => new Predication(predicate, identifier);
}
