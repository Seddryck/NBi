using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class TextToTokenCount : INativeTransformation
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
            if (string.IsNullOrEmpty(value) || value == "(null)" || value == "(empty)" || value == "(blank)")
                return 0;
            else
                return TokenCount(value);
        }

        private int TokenCount(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                int len = value.Length;
                int count = 0;
                bool tokenRunning = false;

                for (int i = 0; i < len; i++)
                {
                    if (char.IsLetterOrDigit(value[i]) || char.Parse("-")==value[i])
                    {
                        if (!tokenRunning)
                            count += 1;
                        tokenRunning = true;
                    }
                    if (char.IsWhiteSpace(value[i]))
                        tokenRunning = false;
                }
                    

                return count;
            }
            else
            {
                return 0;
            }
        }

    }
}
