using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Assemblies.Resource
{
    class CustomCommandWithMulipleCtors : ICustomCommand
    {
        public CustomCommandWithMulipleCtors()
        { }

        public CustomCommandWithMulipleCtors(string name)
        { }

        public CustomCommandWithMulipleCtors(string name, int count)
        { }

        public void Execute() { }
    }
}
