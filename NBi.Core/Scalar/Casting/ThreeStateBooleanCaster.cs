using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    class ThreeStateBooleanCaster : BooleanCaster, ICaster<ThreeStateBoolean>
    {
        public new ThreeStateBoolean Execute(object value)
        {
            if (value is ThreeStateBoolean)
                return (ThreeStateBoolean)value;

            if (value is bool)
                return (bool)value ? ThreeStateBoolean.True : ThreeStateBoolean.False;

            var boolValue = IntParsing(value);
            if (boolValue != ThreeStateBoolean.Unknown)
                return boolValue;

            boolValue = StringParsing(value);
            if (boolValue != ThreeStateBoolean.Unknown)
                return boolValue;

            return ThreeStateBoolean.Unknown;
        }

        public override bool IsValid(object value)
        {
            if (value is ThreeStateBoolean || value is bool)
                return true;

            return (base.IsValid(value) || StringParsing(value) != ThreeStateBoolean.Unknown);
        }

        
    }
}
