using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report
{
    public interface IParser
    {
        string ExtractQuery(IQueryRequest request);
    }
}
