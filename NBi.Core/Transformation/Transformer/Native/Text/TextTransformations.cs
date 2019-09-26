using NBi.Core.Scalar.Casting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                case DBNull _: return EvaluateNull();
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

    abstract class AbstractTextLengthTransformation : AbstractTextTransformation
    {
        public int Length { get; }

        public AbstractTextLengthTransformation(string length)
        {
            var tempLength = new NumericCaster().Execute(length);
            Length = Convert.ToInt32(Math.Truncate(tempLength));
        }
    }

    class TextToFirstChars : AbstractTextLengthTransformation
    {
        public TextToFirstChars(string length)
            : base(length) { }

        protected override object EvaluateString(string value) => value.Length>=Length ? value.Substring(0, Length) : value ;
    }

    class TextToLastChars : AbstractTextLengthTransformation
    {
        public TextToLastChars(string length)
            : base(length) { }

        protected override object EvaluateString(string value) => value.Length >= Length ? value.Substring(value.Length-Length, Length) : value;
    }

    abstract class AbstractTextPadTransformation : AbstractTextLengthTransformation
    {
        public char Character { get; }

        public AbstractTextPadTransformation(string length, string character)
            : base(length)
        {
            Character = character[0];
        }

        protected override object EvaluateEmpty() => new string(Character, Length);
        protected override object EvaluateNull() => new string(Character, Length);

    }

    class TextToPadRight : AbstractTextPadTransformation
    {
        public TextToPadRight(string length, string character)
            : base(length, character) { }

        protected override object EvaluateString(string value) => value.Length >= Length ? value : value.PadRight(Length, Character);
    }

    class TextToPadLeft : AbstractTextPadTransformation
    {
        public TextToPadLeft(string length, string character)
            : base(length, character) { }

        protected override object EvaluateString(string value) => value.Length >= Length ? value : value.PadLeft(Length, Character);
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

    class TextToDateTime : AbstractTextTransformation
    {
        public string Format { get; }
        public DateTimeFormatInfo Info { get; }

        public TextToDateTime(string format)
            =>  (Format, Info) = (format, CultureInfo.InvariantCulture.DateTimeFormat);

        public TextToDateTime(string format, string culture)
            => (Format, Info) = (format, new CultureInfo(culture).DateTimeFormat);

        protected override object EvaluateString(string value)
        {
            if (DateTime.TryParseExact(value, Format, Info, DateTimeStyles.None, out var dateTime))
                return dateTime;

            throw new NBiException($"Impossible to transform the value '{value}' into a date using the format '{Format}'");
        }
    }
}
