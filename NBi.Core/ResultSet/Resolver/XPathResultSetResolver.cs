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
    class XPathResultSetResolver : IResultSetResolver
    {
        private readonly XPathResultSetResolverArgs args;

        public XPathResultSetResolver(XPathResultSetResolverArgs args)
        {
            this.args = args;
        }

        public virtual ResultSet Execute()
        {
            var objects = args.XPathEngine.Execute();

            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(objects);

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
    }
}
