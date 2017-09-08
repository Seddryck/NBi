using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class StringToUpper : INativeTransformation
    {
        public object Evaluate(object value)
        {
            if (value is string || value == null)
                return EvaluateString(value as string);
            else
                throw new NotImplementedException();
        }

        private object EvaluateString(string value)
        {
            if (string.IsNullOrEmpty(value) || (value.StartsWith("(") && value.EndsWith(")")))
                return value;
            else
                return value.ToUpperInvariant();
        }

    }
}
