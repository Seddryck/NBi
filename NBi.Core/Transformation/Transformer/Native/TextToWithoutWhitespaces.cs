using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class TextToWithoutWhitespaces : INativeTransformation
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
            if (string.IsNullOrEmpty(value) || value == "(null)" || value == "(empty)")
                return value;
            else if (value == "(blank)")
                return "(empty)";
            else
                return RemoveWhitespaces(value);
        }

        private string RemoveWhitespaces(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                int len = value.Length;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < len; i++)
                    sb.Append(value[i], char.IsWhiteSpace(value[i]) ? 0 : 1);

                return (sb.ToString());
            }
            else
            {
                return value;
            }
        }

    }
}
