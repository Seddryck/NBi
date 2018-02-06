using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Caster
{
    class BooleanCaster : BaseNumericCaster, ICaster<Boolean>
    {
        public Boolean Execute(object value)
        {
            if (value is Boolean)
                return (Boolean)value;

            if (value is bool)
                return (bool)value;

            var boolValue = IntParsing(value);
            if (boolValue != ThreeStateBoolean.Unknown)
                return boolValue == ThreeStateBoolean.True;

            boolValue = StringParsing(value);
            if (boolValue != ThreeStateBoolean.Unknown)
                return boolValue == ThreeStateBoolean.True;

            throw new ArgumentOutOfRangeException();
        }

        object ICaster.Execute(object value) => Execute(value);


        public override bool IsValid(object value)
        {
            if (value is Boolean || value is bool)
                return true;

            return (base.IsValid(value) || StringParsing(value) != ThreeStateBoolean.Unknown);
        }


        protected ThreeStateBoolean IntParsing(object obj)
        {
            if (IsParsableNumeric(obj))
            {
                var dec = System.Convert.ToDecimal(obj, NumberFormatInfo.InvariantInfo);
                if (dec == new decimal(0))
                    return ThreeStateBoolean.False;
                if (dec == new decimal(1))
                    return ThreeStateBoolean.True;
            }
            return ThreeStateBoolean.Unknown;
        }


        protected ThreeStateBoolean StringParsing(object obj)
        {
            var str = obj.ToString().ToLowerInvariant();
            if (str == "false" || str == "no")
                return ThreeStateBoolean.False;
            if (str == "true" || str == "yes")
                return ThreeStateBoolean.True;
            return ThreeStateBoolean.Unknown;
        }

        protected string ThreeStateToString(ThreeStateBoolean ts, string value)
        {
            switch (ts)
            {
                case ThreeStateBoolean.False:
                    return "false";
                case ThreeStateBoolean.True:
                    return "true";
            }
            return value;
        }
    }
}
