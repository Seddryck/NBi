using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public class Equal : ScalarReferencePredicate
{
    public override ComparerType ComparerType { get => ComparerType.Equal; }
}
