using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening.Xml;

public class XPathArgs : IFlattenizerArgs
{
    public IScalarResolver<string> From { get; set; } = new LiteralScalarResolver<string>(string.Empty);
    public IEnumerable<IPathSelect> Selects { get; set; } = [];
    public string DefaultNamespacePrefix { get; set; } = string.Empty;
    public bool IsIgnoreNamespace { get; set; } = false;

    public XPathArgs() { }
}
