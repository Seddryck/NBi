using NBi.Core.Scalar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    abstract class CultureSensitiveTextPredicate : AbstractPredicate
    {
        protected CultureInfo CultureInfo { get; }

        public CultureSensitiveTextPredicate(bool not, string culture)
            : base(not)
        {
            var factory = new CultureFactory();
            CultureInfo = factory.Instantiate(culture);
        }
    }
}
