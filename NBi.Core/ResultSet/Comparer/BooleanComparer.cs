using NBi.Core.ResultSet.Converter;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class BooleanComparer : BaseComparer
    {
        private readonly IConverter<ThreeStateBoolean> converter;

        public BooleanComparer()
        {
            converter = new BooleanConverter();
        }

        protected override ComparerResult CompareObjects(object x, object y)
        {
            var xThreeState = converter.Convert(x);
            var yThreeState = converter.Convert(y);

            if (IsEqual(xThreeState, yThreeState))
                return ComparerResult.Equality;

            return new ComparerResult(x.ToString());
        }

        protected override ComparerResult CompareObjects(object x, object y, Tolerance tolerance)
        {
            throw new NotImplementedException("You cannot compare two booleans with a tolerance");
        }

        protected override ComparerResult CompareObjects(object x, object y, Rounding rounding)
        {
            throw new NotImplementedException("You cannot compare two booleans with a rounding.");
        }

        protected bool IsEqual(ThreeStateBoolean x, ThreeStateBoolean y)
        {
            if (x == ThreeStateBoolean.Unknown || y == ThreeStateBoolean.Unknown)
                return false;

            //quick check
            return (x == y);
        }

        protected override bool IsValidObject(object x)
        {
            return converter.IsValid(x);
        }
    }
}
