using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource;

class CustomCommandWithTwoParameters : ICustomCommand
{
    public CustomCommandWithTwoParameters(string name, int count)
    { }

    public void Execute() { }
}
