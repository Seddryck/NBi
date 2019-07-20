using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Assemblies.Resource
{
    class CustomCommandWithTwoParameters : ICustomCommand
    {
        public CustomCommandWithTwoParameters(string name, int count)
        { }

        public void Execute() { }
    }
}
