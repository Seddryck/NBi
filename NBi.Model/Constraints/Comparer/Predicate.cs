using NBi.Core.Calculation;
using NBi.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Constraints.Comparer;

public abstract class Predicate
{
    public bool Not { get; set; }

    public ProjectionOld? Projection { get; set; }

    public QueryScalar? QueryScalar { get; set; }

    public abstract ComparerType ComparerType { get; }
}
