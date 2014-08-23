using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    /// <summary>
    /// The string helper.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// To randomize the provided string by appending a random <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">
        /// The original value.
        /// </param>
        /// <returns>
        /// The randomized <see cref="string"/>.
        /// </returns>
        public static string RemoveDiacritics(this string value)
        {
            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static int LevenshteinDistance(this string value, string compared)
        {
            int[,] d = new int[value.Length + 1, compared.Length + 1];
            for (int i = 0; i <= value.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= compared.Length; j++)
                d[0, j] = j;
            for (int j = 1; j <= compared.Length; j++)
                for (int i = 1; i <= value.Length; i++)
                    if (value[i - 1] == compared[j - 1])
                        d[i, j] = d[i - 1, j - 1];  //no operation
                    else
                        d[i, j] = Math.Min(Math.Min(
                            d[i - 1, j] + 1,    //a deletion
                            d[i, j - 1] + 1),   //an insertion
                            d[i - 1, j - 1] + 1 //a substitution
                            );
            return d[value.Length, compared.Length];
        }
    }
}
