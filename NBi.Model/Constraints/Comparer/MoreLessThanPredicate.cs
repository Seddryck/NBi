using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public abstract class MoreLessThanPredicate : ScalarReferencePredicate, ICaseSensitiveTextPredicate
{
    public bool IgnoreCase { get; set; }

    public bool OrEqual { get; set; }
}
