using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch
{
    interface IBatchFatory
    {
        IDecorationCommandImplementation Get(IBatchCommand command, IDbConnection connection);
    }
}
