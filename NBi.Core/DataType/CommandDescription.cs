using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

public class CommandDescription
{
    protected readonly Target target;
    protected readonly IEnumerable<CaptionFilter> filters;

    public Target Target
    {
        get { return target; }
    }

    public IEnumerable<CaptionFilter> Filters
    {
        get { return filters; }
    }

    public CommandDescription(Target target, IEnumerable<CaptionFilter> filters)
    {
        this.target = target;
        this.filters = filters;
    }
}
