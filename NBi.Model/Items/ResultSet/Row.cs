using System.Linq;
using System.Collections.Generic;
using NBi.Core.ResultSet;

namespace NBi.Model.Items.ResultSet;

public class Row : IRow
{
    public List<Cell> _cells { get; set; }

    public IList<ICell> Cells
    {
        get { return _cells.Cast<ICell>().ToList(); }
    }
}
