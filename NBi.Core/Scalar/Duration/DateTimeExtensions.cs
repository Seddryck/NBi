using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration
{
    public static class DateTimeExtensions
    {
        public static DateTime Add(this DateTime dt, IDuration duration)
        {
            if (duration is FixedDuration)
                return dt.Add((duration as FixedDuration).TimeSpan);
            else if (duration is MonthDuration)
                return dt.AddMonths((duration as MonthDuration).Count);
            else if (duration is YearDuration)
                return dt.AddYears((duration as YearDuration).Count);

            throw new ArgumentOutOfRangeException();
        }
    }
}
