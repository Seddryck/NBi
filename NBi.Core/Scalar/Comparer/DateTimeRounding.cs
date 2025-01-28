using System;
using System.Linq;

namespace NBi.Core.Scalar.Comparer;

class DateTimeRounding : Rounding
{
    protected TimeSpan step;

    public DateTimeRounding(TimeSpan step, Rounding.RoundingStyle style)
        : base(step.ToString(), style)
    {
        if (step.Ticks <= 0)
            throw new ArgumentException("The parameter 'step' must be a value greater than zero.", nameof(step));

        if (step.TotalDays > 1)
            throw new ArgumentException("The parameter 'step' must be less or equal to one day", nameof(step));

        this.step = step;
    }

    public DateTime GetValue(DateTime value)
    {
        var newValueMilliSeconds = GetValue(Convert.ToDecimal(value.TimeOfDay.TotalMilliseconds), Convert.ToDecimal(step.TotalMilliseconds));
        return value.Date.AddMilliseconds(Convert.ToDouble(newValueMilliSeconds));
    }
}
