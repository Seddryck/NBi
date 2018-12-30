using NBi.Core.Scalar.Caster;
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

        protected override bool ApplyWithReference(object reference, object x)
        {
            var caster = new DateTimeCaster();
            var dtX = caster.Execute(x);
            var dtY = caster.Execute(reference);

            return Compare(dtX, dtY);
        }

        protected abstract bool Compare(System.DateTime x, System.DateTime y);
    }
}
