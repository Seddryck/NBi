using NBi.Core.ResultSet.Caster;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class BooleanComparer : BaseComparer
    {
        private readonly ICaster<ThreeStateBoolean> caster;

        public BooleanComparer()
        {
            caster = new ThreeStateBooleanCaster();
        }

        protected override ComparerResult CompareObjects(object x, object y)
        {
            var xThreeState = caster.Execute(x);
            var yThreeState = caster.Execute(y);

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
            return caster.IsValid(x);
        }
    }
}
