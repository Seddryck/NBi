using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.PostFilters;

public class IsResultFilter : ICommandFilter, IValueFilter
{
    private string value;
    public string Value { get { return value; } }

    public IsResultFilter(bool isResult)
    {
        this.value = isResult ? "YES" : "NO";
    }
}
