using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

public static class DateTimeExtensions
{
    public static DateTime Add(this DateTime dt, IDuration duration)
        => duration switch
        {
            FixedDuration fix => dt.Add(fix.TimeSpan),
            MonthDuration month => dt.AddMonths(month.Count),
            YearDuration year => dt.AddYears(year.Count), 
            _ => throw new ArgumentOutOfRangeException()
        } ;
}
