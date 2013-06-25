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

            return new ComparerResult(rxText);
        }

        protected bool IsEqual(string x, string y)
        {
            //quick check
            return (x == y);
        }
    }
}
