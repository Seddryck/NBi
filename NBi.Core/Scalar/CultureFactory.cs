using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar
{
    class CultureFactory
    {
        public CultureInfo Invariant { get => Instantiate(string.Empty); }

        public CultureInfo Instantiate(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
                return (new CultureInfo(culture).Clone() as CultureInfo);

            var invariantCulture = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            invariantCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            invariantCulture.DateTimeFormat.DateSeparator = "-";
            return invariantCulture;
        }
    }
}
