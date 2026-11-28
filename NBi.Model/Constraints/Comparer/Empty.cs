using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public class Empty : Predicate
{
    public bool OrNull { get; set; }

    public override ComparerType ComparerType { get => OrNull ? ComparerType.NullOrEmpty : ComparerType.Empty; }
}
