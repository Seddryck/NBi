using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core;

public class ListComparisonFormatter
{

    public ListComparisonFormatter()
    {

    }

    public StringBuilder Format(ListComparer.Result result)
    {
        var sb = new StringBuilder();

        Format("missing", result.Missing, result.MissingCount, ref sb);
        Format("unexpected", result.Unexpected, result.UnexpectedCount, ref sb);
        return sb;
    }

    private void Format(string category, IEnumerable<string> list, int count, ref StringBuilder sb)
    {
        if (list == null)
            return;

        var first = sb.Length;
        sb.AppendFormat("{0}{1} item", !list.Any() ? "no ": string.Empty, category);
        sb.Replace(sb.ToString(first, 1), sb.ToString(first, 1).ToUpper(), first, 1);
        sb.Append('s', Convert.ToInt32(list.Count() > 1));
        if (list.Any())
            sb.AppendFormat(" ({0} of {1})", list.Count(), count);
        sb.AppendLine();

        foreach (var str in list)
        { 
            sb.Append('<');
            sb.Append(str);
            sb.AppendLine(">");
        }
        sb.AppendLine();
    }
}
