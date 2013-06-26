using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class TextComparer
    {
        public ComparerResult Compare(object x, object y)
        {
            var rxText = x.ToString();
            var ryText = y.ToString();

            //Compare decimals (with tolerance)
            if (IsEqual(rxText, ryText))
                return ComparerResult.Equality;

            return new ComparerResult(string.IsNullOrEmpty(rxText) ? "(empty)" : rxText);
        }

        protected bool IsEqual(string x, string y)
        {
            //quick check
            if (x == y)
                return true;

            if (x == "(empty)" && string.IsNullOrEmpty(y))
                return true;

            if (y == "(empty)" && string.IsNullOrEmpty(x))
                return true;

            return false;
        }
    }
}
