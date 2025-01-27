using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

class CommandFilter : ICommandFilter
{
    public CommandFilter(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }
}
