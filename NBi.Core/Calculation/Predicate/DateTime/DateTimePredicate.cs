using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    abstract class DateTimePredicate : AbstractPredicateReference
    {
        public DateTimePredicate(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var converter = new DateTimeConverter();
            var dtX = converter.Convert(x);
            var dtY = converter.Convert(Reference);

            return Compare(dtX, dtY);
        }

        protected abstract bool Compare(System.DateTime x, System.DateTime y);
    }
}
