using NBi.Core.Query;
using NBi.Core.Hierarchical.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Hierarchical;

namespace NBi.Core.ResultSet.Resolver
{
    public class XPathResultSetResolverArgs : ResultSetResolverArgs
    {
        public AbstractPathEngine XPathEngine { get; }

        public XPathResultSetResolverArgs(AbstractPathEngine xpathEngine)
            => XPathEngine = xpathEngine;
    }
}
