using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    public abstract class CultureSensitiveTextPredicate : IPredicate
    {
        protected CultureInfo CultureInfo { get; }

        public CultureSensitiveTextPredicate(string culture)
        {
            CultureInfo = GetCultureInfo(culture);
            if (CultureInfo.LCID == CultureInfo.InvariantCulture.LCID)
            {
                CultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                CultureInfo.DateTimeFormat.DateSeparator = "-";
            }
        }

        public abstract bool Apply(object x);
        private CultureInfo GetCultureInfo(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
                return (new CultureInfo(culture).Clone() as CultureInfo);
            else
                return CultureInfo.InvariantCulture.Clone() as CultureInfo;
        }
    }
}
