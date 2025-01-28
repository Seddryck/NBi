using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.PostFilters;

public class OwnerFilter : ICommandFilter, IValueFilter
{
    private string value;
    public string Value { get { return value; } }

    public OwnerFilter(string value)
    {
        this.value = value;
    }
}
