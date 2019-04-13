using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractTextTransformation : INativeTransformation
    {
        public object Evaluate(object value)
        {
            switch (value)
            {
                case null: return EvaluateNull();
                case DBNull dbnull: return EvaluateNull();
                case string s: return EvaluateHighLevelString(s);
                default:
                    var caster = new TextCaster();
                    var str = caster.Execute(value);
                    return EvaluateHighLevelString(str);
            }
        }

        protected virtual object EvaluateHighLevelString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return EvaluateEmpty();

            if (value.Equals("(null)"))
                return EvaluateNull();

            if (value.Equals("(empty)") || string.IsNullOrEmpty(value))
                return EvaluateEmpty();

            if (value.Equals("(blank)") || string.IsNullOrWhiteSpace(value))
                return EvaluateBlank();

            if (value.StartsWith("(") && value.EndsWith(")"))
                return EvaluateSpecial(value);

            return EvaluateString(value as string);
        }

        protected virtual object EvaluateNull() => "(null)";
        protected virtual object EvaluateEmpty() => "(empty)";
        protected virtual object EvaluateBlank() => "(blank)";
        protected virtual object EvaluateSpecial(string value) => value; 
        protected abstract object EvaluateString(string value);
    }

    class TextToHtml : AbstractTextTransformation
    {
        protected override object EvaluateString(string value) => WebUtility.HtmlEncode(value);
    }

    class TextToLower : AbstractTextTransformation
    {
        protected override object EvaluateString(string value) => value.ToLowerInvariant();
    }
    class TextToUpper : AbstractTextTransformation
    {
        protected override object EvaluateString(string value) => value.ToUpperInvariant();
    }

    class TextToTrim : AbstractTextTransformation
    {
        protected override object EvaluateBlank() => "(empty)";
        protected override object EvaluateString(string value) => value.Trim();
    }

    class BlankToEmpty : AbstractTextTransformation
    {
        protected override object EvaluateBlank() => "(empty)";
        protected override object EvaluateString(string value) => value;
    }

    class BlankToNull : AbstractTextTransformation
    {
        protected override object EvaluateBlank() => "(null)";
        protected override object EvaluateEmpty() => "(null)";
        protected override object EvaluateString(string value) => value;
    }

    class EmptyToNull : AbstractTextTransformation
    {
        protected override object EvaluateEmpty() => "(null)";
        protected override object EvaluateString(string value) => value;
    }

    class NullToEmpty : AbstractTextTransformation
    {
        protected override object EvaluateNull() => "(empty)";
        protected override object EvaluateString(string value) => value;
    }

    class HtmlToText : AbstractTextTransformation
    {
        protected override object EvaluateString(string value) => WebUtility.HtmlDecode(value);
    }

    class TextToLength : AbstractTextTransformation
    {
        protected override object EvaluateSpecial(string value) => -1;
        protected override object EvaluateBlank() => -1;
        protected override object EvaluateEmpty() => 0;
        protected override object EvaluateNull() => 0;
        protected override object EvaluateString(string value) => value.Length;
    }

    class TextToTokenCount : TextToLength
    {
        protected override object EvaluateBlank() => 0;
        protected override object EvaluateString(string value) => TokenCount(value);

        private int TokenCount(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                int len = value.Length;
                int count = 0;
                bool tokenRunning = false;

                for (int i = 0; i < len; i++)
                {
                    if (char.IsLetterOrDigit(value[i]) || char.Parse("-") == value[i])
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
