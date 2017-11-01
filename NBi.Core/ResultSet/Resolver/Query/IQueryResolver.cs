using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
    interface IQueryResolver
    {
        IDbCommand Execute();
    }
}
