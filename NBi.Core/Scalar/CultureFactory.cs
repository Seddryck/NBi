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
                return (new CultureInfo(culture));

            var invariantCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            invariantCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            invariantCulture.DateTimeFormat.DateSeparator = "-";
            return invariantCulture;
        }
    }
}
