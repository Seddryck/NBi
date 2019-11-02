using NBi.Core.Query;
using NBi.Core.Hierarchical.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class XPathResultSetResolver : IResultSetResolver
    {
        private XPathResultSetResolverArgs Args { get; }

        public XPathResultSetResolver(XPathResultSetResolverArgs args)
            => Args = args;

        public virtual ResultSet Execute()
        {
            var objects = Args.XPathEngine.Execute();

            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(objects);

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
    }
}
