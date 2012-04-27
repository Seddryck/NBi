using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Query
{
    public interface IResultSetComparer
    {
        Result Validate(string path);
    }
}
