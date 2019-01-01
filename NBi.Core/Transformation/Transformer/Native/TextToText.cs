using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractTextToText : INativeTransformation
    {
        public object Evaluate(object value)
        {
            if (!(value is string) && value!=null)
                throw new NotImplementedException();
            var str = value as string;

            if (string.IsNullOrEmpty(str) || (str.StartsWith("(") && str.EndsWith(")")))
                return SpecialValue(str);
            else 
                return EvaluateString(value as string);
        }

        protected virtual object SpecialValue(string value) => value; 
        protected abstract object EvaluateString(string value);
    }

    class TextToHtml : AbstractTextToText
    {
        protected override object EvaluateString(string value) => WebUtility.HtmlEncode(value);
    }

    class TextToLower : AbstractTextToText
    {
        protected override object EvaluateString(string value) => value.ToLowerInvariant();
    }
    class TextToUpper : AbstractTextToText
    {
        protected override object EvaluateString(string value) => value.ToUpperInvariant();
    }

    class TextToTrim : AbstractTextToText
    {
        protected override object SpecialValue(string value) => value == "(blank)" ? "(empty)" : value;
        protected override object EvaluateString(string value) => value.Trim();
    }


    //TODO !!! ==> 
    /*
     * COntinue to move all the more or less equivalent Native Transformers to inherit from the same abstract classes
     * Add something that will determine the expected input for these native transformers (Text, Numeric, Boolean, DateTime)
     * Remove the pseudo-work-around for ScalarResolverArgsBuilder at line 98
     */
}
