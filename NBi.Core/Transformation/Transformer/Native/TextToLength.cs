using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class TextToLength : INativeTransformation
    {
        public object Evaluate(object value)
        {
            if (value == null)
                return 0;
            else if (value is string)
                return EvaluateString(value as string);
            else
                throw new NotImplementedException();
        }

        private object EvaluateString(string value)
        {
            if (value == "(any)" || value == "(value)" || value == "(blank)")
                return -1;
            else if (string.IsNullOrEmpty(value) || value == "(null)" || value == "(empty)")
                return 0;
            else
                return value.Length;
        }
        
    }
}
