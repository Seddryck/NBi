using System;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class DateTimeRounding : Rounding
    {
        protected TimeSpan step;

        public DateTimeRounding(TimeSpan step, Rounding.RoudingStyle style)
            : base(step.ToString(), style)
        {
            if (step.Ticks <= 0)
                throw new ArgumentException("The parameter 'step' must be a value greater than zero.", "step");

            if (step.TotalDays > 1)
                throw new ArgumentException("The parameter 'step' must be less or equal to one day", "step");

            this.step = step;
        }

        public DateTime GetValue(DateTime value)
        {
            var newValueMilliSeconds = GetValue(value.TimeOfDay.TotalMilliseconds, step.TotalMilliseconds);
            return value.Date.AddMilliseconds(newValueMilliSeconds);
        }
    }
}
