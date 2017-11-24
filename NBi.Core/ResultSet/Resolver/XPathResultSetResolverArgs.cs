using NBi.Core.Query;
using NBi.Core.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class XPathResultSetResolverArgs : ResultSetResolverArgs
    {
        public XPathEngine XPathEngine { get; }

        public XPathResultSetResolverArgs(XPathEngine xpathEngine)
        {
            this.XPathEngine = xpathEngine;
        }
    }
}
