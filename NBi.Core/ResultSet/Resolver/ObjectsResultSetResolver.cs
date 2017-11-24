using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ObjectsResultSetResolver : IResultSetResolver
    {
        private readonly ObjectsResultSetResolverArgs args;

        public ObjectsResultSetResolver(ObjectsResultSetResolverArgs args)
        {
            this.args = args;
        }

        public virtual ResultSet Execute()
        {
            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(args.Objects);

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }

    }
}
