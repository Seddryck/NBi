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
            CultureInfo = GetCultureInfo(culture);
            if (CultureInfo.LCID == CultureInfo.InvariantCulture.LCID)
            {
                CultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                CultureInfo.DateTimeFormat.DateSeparator = "-";
            }
        }

        private CultureInfo GetCultureInfo(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
                return (new CultureInfo(culture).Clone() as CultureInfo);
            else
                return CultureInfo.InvariantCulture.Clone() as CultureInfo;
        }
    }
}
