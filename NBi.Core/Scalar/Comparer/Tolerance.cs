using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

public abstract class Tolerance
{
    public virtual string ValueString { get; private set; }
    
    protected Tolerance(string value)
    {
        ValueString = value;
    }

    public static bool IsNullOrNone(Tolerance? tolerance)
    {
        return (tolerance == null
            || tolerance == DateTimeTolerance.None
            || tolerance == TextTolerance.None
            || tolerance == NumericAbsoluteTolerance.None);
    }
}
