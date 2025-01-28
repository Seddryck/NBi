using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exssif = Expressif.Predicates;

namespace NBi.Core.Calculation.Asserting;

internal class Predicate(Exssif.IPredicate predicate, bool negate = false) : IPredicate
{
    protected Exssif.IPredicate InternalPredicate { get; } = predicate;
    protected bool Negate { get; } = negate;
    public bool Execute(object? x)
        => Negate ? !InternalPredicate.Evaluate(x) : InternalPredicate.Evaluate(x);
}
