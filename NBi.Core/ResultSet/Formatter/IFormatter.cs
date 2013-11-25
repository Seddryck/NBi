using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet.Formatter
{
    public interface IFormatter
    {
        string Tabulize(IEnumerable<DataRow> rows);
    }
}
