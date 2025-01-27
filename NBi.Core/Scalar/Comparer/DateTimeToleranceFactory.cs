using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Comparer;

class DateTimeToleranceFactory
{
    public DateTimeTolerance Instantiate(string value)
    {
        return new DateTimeTolerance(TimeSpan.Parse(value));
    }
}
