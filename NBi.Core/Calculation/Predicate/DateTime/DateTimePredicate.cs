using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    abstract class DateTimePredicate : IPredicate
    {
        public bool Compare(object x, object y)
        {
            var converter = new DateTimeConverter();
            var dtX = converter.Convert(x);
            var dtY = converter.Convert(y);

            return Compare(dtX, dtY);
        }

        public abstract bool Compare(System.DateTime x, System.DateTime y);
    }
}
