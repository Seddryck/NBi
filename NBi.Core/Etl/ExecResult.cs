using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Etl
{
    public enum ExecResult : int
    {
        Failure = 1,
        Success = 0,
        Completion = 2,
        Canceled = 3,
    }
}
