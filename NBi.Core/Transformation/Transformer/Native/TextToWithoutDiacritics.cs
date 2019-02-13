using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class TextToWithoutDiacritics : AbstractTextToText
    {
        protected override object EvaluateString(string value) => RemoveDiacritics(value);

        /// <summary>
        /// Remove diacritics from string. Origin value can be null and String.Empty.
        /// Source code from https://mmlib.codeplex.com/SourceControl/latest#MMLib.Extensions/StringExtensions.cs
        /// </summary>
        /// <param name="value">Origin string value. </param>
        /// <returns>New string value without diacritics.</returns>
        private string RemoveDiacritics(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                string stFormD = value.Normalize(NormalizationForm.FormD);
                int len = stFormD.Length;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < len; i++)
                {
                    System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(stFormD[i]);
                    }
                }

                return (sb.ToString().Normalize(NormalizationForm.FormC));
            }
            else
            {
                return value;
            }
        }
    }
}
