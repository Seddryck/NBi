using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextEqual : AbstractPredicateReference
    {
        public TextEqual(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var cpr = new TextComparer();
            return cpr.Compare(x, Reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to '{Reference}'";
        }
    }
}
