using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening;

public class AttributeSelect: ElementSelect
{
    public string Attribute { get; }

    internal AttributeSelect(IScalarResolver<string> path, string attribute)
        : base(path)
        => Attribute = attribute;
}
