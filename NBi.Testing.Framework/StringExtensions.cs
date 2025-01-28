using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Framework.Testing;

public static class StringExtensions
{
    public static IEnumerable<int> IndexOfAll(this string sourceString, string subString)
        => Regex.Matches(sourceString, subString).Cast<Match>().Select(m => m.Index);
    public static IEnumerable<int> IndexOfAll(this string sourceString, char subString)
        => sourceString.IndexOfAll(subString.ToString());
}
