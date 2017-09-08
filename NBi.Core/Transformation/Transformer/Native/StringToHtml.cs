using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class StringToHtml : INativeTransformation
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
                return value;
            else
                return WebUtility.HtmlEncode(value);
        }
    }
    }
