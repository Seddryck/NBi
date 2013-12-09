using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Format
{
    public class RegexBuilder
    {

        public string Build(INumericFormat format)
        {
            var regex = string.Empty;

            if (format.DecimalDigits == 0)
            {
                //No decimal
                regex = string.Format(@"$");
            }
            else
            { 
                //Decimal Separator
                regex = string.Format(@"\{0}", format.DecimalSeparator);

                //Decimal digits
                regex = string.Format(@"{0}[0-9]{{{1}}}$", regex, format.DecimalDigits);
            }

            //Group Separator
            if (string.IsNullOrEmpty(format.GroupSeparator))
                regex = string.Format(@"^?[0-9]*{0}", regex);
            else if (string.IsNullOrWhiteSpace(format.GroupSeparator))
                regex = string.Format(@"^?[0-9]{{1,3}}(?:\{1}?[0-9]{{3}})*{0}", regex, "s");
            else
                regex = string.Format(@"^?[0-9]{{1,3}}(?:\{1}?[0-9]{{3}})*{0}", regex, format.GroupSeparator);

            return regex;
        }

        public string Build(ICurrencyFormat format)
        {
            var regexCurrency = string.Empty;
            switch (format.CurrencyPattern)
            {
                case CurrencyPattern.Prefix: regexCurrency = @"\{1}{0}";
                    break;
                case CurrencyPattern.Suffix: regexCurrency= @"{0}\{1}";
                    break;
                case CurrencyPattern.PrefixSpace: regexCurrency= @"\{1}\s{0}";
                    break;
                case CurrencyPattern.SuffixSpace: regexCurrency= @"{0}\s\{1}";
                    break;
                default:
                    break;
            }

            var regex = Build((INumericFormat)format);
            regex = regex.Remove(regex.Length - 1, 1).Remove(0, 1);
            regex = string.Format("^" + regexCurrency + "$", regex, format.CurrencySymbol);

            return regex;
        }
    }
}
