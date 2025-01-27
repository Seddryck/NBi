using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational.PostFilters;

public class ParameterDirectionFilter : ICommandFilter, IValueFilter
{
    private string value;
    public string Value { get { return value; } }
    public ParameterDirectionFilter(string direction)
    {
        this.value = direction.ToUpperInvariant();
    }
}
