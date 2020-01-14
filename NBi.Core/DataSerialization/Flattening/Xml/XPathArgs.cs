using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening.Xml
{
    public class XPathArgs : IFlattenizerArgs
    {
        public string From { get; set; }
        public IEnumerable<IPathSelect> Selects { get; set; } = new List<IPathSelect>();
        public string DefaultNamespacePrefix { get; set; }
        public bool IsIgnoreNamespace { get; set; } = false;

        public XPathArgs() { }
    }
}
