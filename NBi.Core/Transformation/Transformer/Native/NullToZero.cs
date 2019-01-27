using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NullToZero : INativeTransformation
    {

        public NullToZero()
        { }

        public object Evaluate(object value)
        {
            if (value == null || DBNull.Value.Equals(value) || value as string == "(empty)" || value as string == "(null)" || value is string && string.IsNullOrEmpty(value as string))
                return 0;
            else
                return value;
        }
    }
}
