using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure;

public class CommandDescription
{
    protected readonly Target target;
    protected readonly IEnumerable<IFilter> filters;

    public Target Target
    {
        get { return target; }
    }

    public IEnumerable<IFilter> Filters
    {
        get { return filters; }
    }

    public CommandDescription(Target target, IEnumerable<IFilter> filters)
    {
        this.target = target;
        this.filters = filters;
    }
}
