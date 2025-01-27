using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

public interface IContent
{
    IList<IRow> Rows { get; set; }
    IList<string> Columns { get; set; }
}
