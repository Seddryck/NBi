using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Boolean
{
    class BooleanTrue : AbstractPredicate
    {
        public BooleanTrue(bool not)
               : base(not)
        { }

        protected override bool Apply(object x)
        {
            var cpr = new BooleanComparer();
            return cpr.Compare(x, true).AreEqual;
        }

        public override string ToString() => $"is true";
    }
}
