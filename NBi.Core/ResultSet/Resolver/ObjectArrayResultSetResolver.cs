using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ObjectArrayResultSetResolver : IResultSetResolver
    {
        private readonly object[] objects;

        public ObjectArrayResultSetResolver(object[] objects)
        {
            this.objects = objects;
        }

        public virtual ResultSet Execute()
        {
            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(objects);

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }

    }
}
