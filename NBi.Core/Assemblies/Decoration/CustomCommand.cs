using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies.Decoration;

class CustomCommand : IDecorationCommand
{
    private ICustomCommand Target { get; }

    public CustomCommand(ICustomCommand target) => Target = target;

    public void Execute() => Target.Execute();
}
