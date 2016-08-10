using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch
{
    public interface IBatchRunnerFatory
    {
        IDecorationCommandImplementation Get(IBatchCommand command, IDbConnection connection);
    }
}
