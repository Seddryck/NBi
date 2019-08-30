using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class ValueToValue : INativeTransformation
    {
        public object Evaluate(object value)
        {
            if (value == null || DBNull.Value.Equals(value) || (value is string && value as string == "(null)"))
                return "(null)";
            else
                return "(value)";
        }
        
    }
}
