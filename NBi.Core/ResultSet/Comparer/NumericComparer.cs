using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class NumericComparer
    {
        public ComparerResult Compare(object x, object y)
        {
            return Compare(x, y, 0);
        }
        
        public ComparerResult Compare(object x, object y, decimal tolerance)
        {
            var rxDecimal = Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo);
            var ryDecimal = Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo);

            //Compare decimals (with tolerance)
            if (IsEqual(rxDecimal, ryDecimal, tolerance))
                return ComparerResult.Equality;

            return new ComparerResult(rxDecimal.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected bool IsEqual(decimal x, decimal y, decimal tolerance)
        {
            //Console.WriteLine("IsEqual: {0} {1} {2} {3} {4} {5}", x, y, tolerance, Math.Abs(x - y), x == y, Math.Abs(x - y) <= tolerance);

            //quick check
            if (x == y)
                return true;

            //Stop checks if tolerance is set to 0
            if (tolerance == 0)
                return false;

            //include some math[Time consumming] (Tolerance needed to validate)
            return (Math.Abs(x - y) <= tolerance);
        }
    }
}
