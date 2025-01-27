using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies.Decoration;

public class CustomCommandFactory : AbstractCustomFactory<ICustomCommand>
{
    protected override string CustomKind => "custom command in a setup or cleanup";

    public IDecorationCommand Instantiate(CustomCommandArgs args)
        => new CustomCommand(base.Instantiate(args));
}
