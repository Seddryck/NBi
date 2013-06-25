using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class BooleanComparer : BaseComparer
    {
        public ComparerResult Compare(object x, object y)
        {
            var xThreeState = IntParsing(x);
            if (xThreeState == ThreeState.Unknown)
                xThreeState = StringParsing(x);

            var yThreeState = IntParsing(y);
            if (yThreeState == ThreeState.Unknown)
                yThreeState = StringParsing(y);

            if (IsEqual(xThreeState, yThreeState))
                return ComparerResult.Equality;

            return new ComparerResult(ThreeStateToString(xThreeState));
        }

        protected ThreeState IntParsing(object obj)
        {
            if (IsValidNumeric(obj))
            {
                var dec = Convert.ToDecimal(obj, NumberFormatInfo.InvariantInfo);
                if (dec == new decimal(0))
                    return ThreeState.False;
                if (dec == new decimal(1))
                    return ThreeState.True;   
            }
            return ThreeState.Unknown;
        }


        protected ThreeState StringParsing(object obj)
        {
            var str= obj.ToString().ToLowerInvariant();
            if (str == "false")
                return ThreeState.False;
            if (str == "true")
                return ThreeState.True;
            return ThreeState.Unknown;
        }

        protected enum ThreeState
        {
            Unknown = -1,
            False = 0,
            True= 1
        };


        protected string ThreeStateToString(ThreeState ts)
        {
            switch (ts)
            {
                case ThreeState.False:
                    return "false";
                case ThreeState.True:
                    return "true";
            }
            return "unknown";
        }

        protected bool IsEqual(ThreeState x, ThreeState y)
        {
            if (x == ThreeState.Unknown || y == ThreeState.Unknown)
                return false;

            //quick check
            return (x == y);
        }
    }
}
