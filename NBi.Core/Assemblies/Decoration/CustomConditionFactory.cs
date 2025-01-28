using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies.Decoration;

public class CustomConditionFactory : AbstractCustomFactory<ICustomCondition>
{
    protected override string CustomKind => "custom condition";

    public IDecorationCondition Instantiate(ICustomConditionArgs args)
        => new CustomCondition(base.Instantiate(args));
}
