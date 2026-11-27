using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public class MatchesDate : Predicate, ICultureSensitiveTextPredicate
{
    public string? Culture { get; set; }

    public override ComparerType ComparerType { get => ComparerType.MatchesDate; }

}
