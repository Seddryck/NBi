using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Assemblies.Resource
{
    class CustomCommandWithoutParameter : ICustomCommand
    {
        public CustomCommandWithoutParameter()
        { }

        public void Execute() { }
    }
}
