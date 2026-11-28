using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public class EndsWith : ScalarReferencePredicate, ICaseSensitiveTextPredicate
{
    public bool IgnoreCase { get; set; }
    public override ComparerType ComparerType { get => ComparerType.EndsWith; }
}
