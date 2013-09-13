using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class NumericComparer : BaseComparer
    {
        public ComparerResult Compare(object x, object y, string tolerance)
        {
            return base.Compare(x, y, tolerance);
        }
        
        public ComparerResult Compare(object x, object y, decimal tolerance)
        {
            return base.Compare(x, y, tolerance);
        }

        protected override ComparerResult CompareObjects(object x, object y)
        {
            return CompareObjects(x, y, (decimal)0);
        }

        protected override ComparerResult CompareObjects(object x, object y, object tolerance)
        {
            var rxDecimal = Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo);
            var ryDecimal = Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo);

            if (!(tolerance is decimal) && !(tolerance is string))
                throw new ArgumentException(string.Format("Tolerance for a numeric comparer must be a decimal or a string and is a '{0}'.", tolerance.GetType().Name), "tolerance");

            //Convert the value to an absolute decimal value
            decimal toleranceDecimal =0 ;
            var isDecimal= false;
            if ((tolerance is decimal))
            {
                toleranceDecimal = (decimal)tolerance;
                isDecimal=true;
            }
            else
                isDecimal= decimal.TryParse((string)tolerance, NumberStyles.Float ,NumberFormatInfo.InvariantInfo, out toleranceDecimal);

            //Convert the value to an % decimal value
            decimal tolerancePercentage=0;
            var isPercentage = false;
            if (!isDecimal && ((string)tolerance).Replace(" ", "").Reverse().ElementAt(0) == '%')
            {
                var percentage = string.Concat(((string)tolerance).Replace(" ", "").Reverse().Skip(1).Reverse());
                isPercentage = decimal.TryParse(percentage, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out tolerancePercentage);
            }
            
            //Ensure an absolute value;
            decimal toleranceValue = 0;
            if (isDecimal)
                toleranceValue = toleranceDecimal;
            else if (isPercentage)
                toleranceValue = rxDecimal * tolerancePercentage/100;
            else
                throw new ArgumentException(string.Format("Tolerance for a numeric comparer must be a decimal or a percentage but '{0}' is not recognized as a valid numeric or percentage value.", tolerance), "tolerance");

            //Compare decimals (with tolerance)
            if (IsEqual(rxDecimal, ryDecimal, toleranceValue))
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


        protected override bool IsValidObject(object x)
        {
            return (IsValidNumeric(x));
        }
    }
}
