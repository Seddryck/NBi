using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ObjectsResultSetResolverArgs : ResultSetResolverArgs
    {
        public IEnumerable<object?> Objects { get; }

        public ObjectsResultSetResolverArgs(IEnumerable<object?> objects)
        {
            Objects = objects;
        }
    }
}
