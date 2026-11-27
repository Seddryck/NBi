using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public class OnTheHour : Predicate
{
    public override ComparerType ComparerType { get => ComparerType.OnTheHour; }
}
